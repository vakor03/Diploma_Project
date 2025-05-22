using System.Collections.Generic;
using _Project.Features.DamageModule;
using _Project.Features.EnemyModule.BasicBehaviour;
using _Project.Features.EnemyModule.BehaviourTrees;
using _Project.Features.EnemyModule.Blackboard;
using _Project.Features.LevelGeneratorModule;
using _Project.Features.StatsModule;
using _Project.Features.WeaponModule;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Features.EnemyModule {
    public class BigGuardRobotAI : EnemyAI {
        [Inject] private EntityWeaponDataHolder _weaponDataHolder;
        [Inject] private IBlockGroupService _blockGroupService;
        [Inject] private IStatService<EntityStats> _statService;
        
        [SerializeField] private float _turretDeactivationDelay = 5f;
        [SerializeField] private float _reloadTime = 10f;
        [SerializeField] private float _timeScaleModifier = 10f;
        [SerializeField] private EnemyMovement _enemyMovement;

        [Header("Animation")]
        [SerializeField] private BigGuardRobotAnimationController _animationController;

        private bool _fireWeapons;
        private bool _isActive = true;
        private bool _isReloading = false;
        private float _reloadStartTime;
        
        private BlackboardKey _playerInRangeKey;
        private BlackboardKey _turretActiveKey;
        private BlackboardKey _noPlayerInRangeTimerKey;
        private BlackboardKey _reloadTimestampKey;
        private BlackboardKey _targetPointKey;
        private List<Vector2Int> _patrolPoints = new List<Vector2Int>();

        protected override void SetupTreeComponents(BehaviourTree behaviourTree) {
            // Get animation controller if not assigned
            if (_animationController == null) {
                _animationController = GetComponent<BigGuardRobotAnimationController>();
            }

            // Initialize Blackboard keys
            _playerInRangeKey = Blackboard.GetOrRegisterKey(BlackboardKeys.Bool.IS_PLAYER_IN_RANGE);
            _turretActiveKey = Blackboard.GetOrRegisterKey(BlackboardKeys.Bool.IS_TURRET_ACTIVE);
            _noPlayerInRangeTimerKey = Blackboard.GetOrRegisterKey(BlackboardKeys.Float.NO_PLAYER_IN_RANGE_TIMER);
            _reloadTimestampKey = Blackboard.GetOrRegisterKey("ReloadTimestamp");
            _targetPointKey = Blackboard.GetOrRegisterKey(BlackboardKeys.Vector2Int.TARGET_POINT);
            
            // Set initial values
            Blackboard.SetValue(_turretActiveKey, true);
            Blackboard.SetValue(_noPlayerInRangeTimerKey, 0f);
            Blackboard.SetValue(_reloadTimestampKey, 0f);
            
            Node rootNode = new PrioritySelector("Root");

            // Zero health reload sequence
            Sequence zeroHealthSequence = new Sequence("ZeroHealthReloadSequence", 4);
            zeroHealthSequence.AddChild(new Leaf("CheckZeroHealth", new Condition(() => {
                float currentHealth = _statService.GetStat(EntityStats.CurrentHealth);
                return currentHealth <= 0f;
            })));
            zeroHealthSequence.AddChild(new Leaf("StopMovement", new ActionStrategy(() => {
                _enemyMovement.Stop();
            })));
            zeroHealthSequence.AddChild(new Leaf("StartReload", new ActionStrategy(() => {
                StartReload();
            })));
            
            // Reload sequence
            Sequence reloadSequence = new Sequence("ReloadSequence", 3);
            reloadSequence.AddChild(new Leaf("CheckReloading", new Condition(() => _isReloading)));
            reloadSequence.AddChild(new Leaf("SetFireWeaponsFalse", new ActionStrategy(() => _fireWeapons = false)));
            reloadSequence.AddChild(new Leaf("CheckReloadTime", new Condition(() => {
                bool reloadComplete = (Time.time - _reloadStartTime) >= _reloadTime;
                if (reloadComplete) {
                    Debug.Log($"Reload complete. Time elapsed: {Time.time - _reloadStartTime}");
                }
                return reloadComplete;
            })));
            reloadSequence.AddChild(new Leaf("FinishReload", new ActionStrategy(() => {
                FinishReload();
            })));
     
            // Player in range sequence
            Sequence playerInRangeSequence = new Sequence("PlayerInRangeSequence", 2);
            playerInRangeSequence.AddChild(new Leaf("CheckActive", new Condition(() => _isActive)));
            playerInRangeSequence.AddChild(new Leaf("CheckPlayerInRange", new Condition(() => 
                Blackboard.TryGetValue(_playerInRangeKey, out bool inRange) && inRange)));
            playerInRangeSequence.AddChild(new Leaf("ResetTimer", new ActionStrategy(() => 
                Blackboard.SetValue(_noPlayerInRangeTimerKey, 0f))));
            playerInRangeSequence.AddChild(new Leaf("StopMovement", new ActionStrategy(() => {
                _enemyMovement.Stop();
            })));
            playerInRangeSequence.AddChild(new Leaf("SetFireWeapons", new ActionStrategy(() => {
                _fireWeapons = true;
                Debug.Log("Setting fire weapons to true");
            })));
            
            // Patrol sequence
            Sequence patrolSequence = new Sequence("PatrolSequence", 1);
            patrolSequence.AddChild(new Leaf("CheckActive", new Condition(() => _isActive)));
            patrolSequence.AddChild(new Leaf("CheckPlayerNotInRange", new Condition(() => 
                Blackboard.TryGetValue(_playerInRangeKey, out bool inRange) && !inRange)));
            patrolSequence.AddChild(new Leaf("SetFireWeaponsFalse", new ActionStrategy(() => _fireWeapons = false)));
            patrolSequence.AddChild(new Leaf("SetPatrolSpeed", new StatChangeStrategy(_statService, 
                EntityStats.CurrentSpeed, _statService.GetStat(EntityStats.PatrolSpeed))));
            patrolSequence.AddChild(new Leaf("Patrol", new PatrolStrategy(_enemyMovement, _patrolPoints, 2f)));
            
            rootNode.AddChild(zeroHealthSequence); 
            rootNode.AddChild(reloadSequence);
            rootNode.AddChild(playerInRangeSequence);
            rootNode.AddChild(patrolSequence);

            behaviourTree.AddChild(rootNode);
        }

        private void StartReload() {
            if (!_isReloading) {
                _isActive = false;
                _isReloading = true;
                _reloadStartTime = Time.time;
                Blackboard.SetValue(_turretActiveKey, false);
                Blackboard.SetValue(_reloadTimestampKey, Time.time);
                
                // Restore health immediately
                _statService.SetStat(EntityStats.CurrentHealth, _statService.GetStat(EntityStats.MaxHealth));
                
                // Play turn off animation
                _animationController?.PlayTurnOff();
                
                Debug.Log("Turret entering reload state - health restored");
            }
        }

        private void FinishReload() {
            _isActive = true;
            _isReloading = false;
            Blackboard.SetValue(_turretActiveKey, true);
            
            // Play turn on animation
            _animationController?.PlayTurnOn();
            
            Debug.Log("Turret reload finished - back online");
        }

        protected override void Update() {
            if (_patrolPoints.Count == 0)
                GetPatrolPoints();
                
            base.Update();
            
            // Update animations
            UpdateAnimations();
            
            // Fire weapons
            if (_fireWeapons) {
                FireWeapons();
            }

            _fireWeapons = false;
        }

        private void UpdateAnimations() {
            if (_animationController == null) return;

            // Don't update animations while reloading (let turn off/on animations play)
            if (_isReloading) {
                return;
            }

            // Only update movement animations if active
            if (_isActive) {
                bool isMoving = _enemyMovement.IsMoving;
                
                if (isMoving) {
                    _animationController.PlayMove();
                } else {
                    _animationController.PlayIdle();
                }
            }
        }

        private void GetPatrolPoints() {
            BlockGroup group = _blockGroupService.FindGroupContaining(new Vector2Int(Mathf.RoundToInt(transform.position.x),
                Mathf.RoundToInt(transform.position.y)));
            _patrolPoints.Clear();

            if (group != null && group.Blocks.Count > 0) {
                Vector2Int leftmost = group.Blocks[0];
                Vector2Int rightmost = group.Blocks[0];

                foreach (Vector2Int block in group.Blocks) {
                    if (block.x < leftmost.x)
                        leftmost = block;
                    if (block.x > rightmost.x)
                        rightmost = block;
                }

                _patrolPoints.Add(leftmost);
                _patrolPoints.Add(rightmost);
                
                Debug.Log($"Set patrol points: {leftmost} to {rightmost}");
            }
        }

        private void OnDrawGizmos() {
            if (!Application.isPlaying) return;

            Blackboard.TryGetValue(_playerInRangeKey, out bool inRange);
            
            Color gizmoColor = Color.red;
            if (_isActive && inRange)
                gizmoColor = Color.green;
            else if (_isActive)
                gizmoColor = Color.yellow;
            else if (_isReloading)
                gizmoColor = Color.blue;
                
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position + Vector3.up*.5f, 0.1f);
            
            if (_blockGroupService == null) return;
                
            BlockGroup group = _blockGroupService.FindGroupContaining(
                new Vector2Int((int)transform.position.x, (int)transform.position.y));
                
            if (group != null) {
                foreach (Vector2Int groupBlock in group.Blocks) {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireCube(new Vector3(groupBlock.x, groupBlock.y, 0), Vector3.one * 0.3f);
                }
            }
        }

        [Button]
        public bool SwitchTurretActive(bool isActive) {
            if (_isActive == isActive) return false;
            
            _isActive = isActive;
            Blackboard.SetValue(_turretActiveKey, isActive);
            
            if (isActive) {
                _animationController?.PlayTurnOn();
            } else {
                _animationController?.PlayTurnOff();
            }
            
            return true;
        }

        private void FireWeapons() {
            if (_weaponDataHolder?.EquippedWeapons == null) {
                Debug.LogWarning("WeaponDataHolder or EquippedWeapons is null");
                return;
            }

            foreach (IWeapon weapon in _weaponDataHolder.EquippedWeapons) {
                switch (weapon) {
                    case IShootable shootable:
                        shootable.Shoot();
                        break;
                }
            }
        }

        // Animation testing methods
        [Button, FoldoutGroup("Animation Testing")]
        public void TestTurnOn() => _animationController?.PlayTurnOn();
        
        [Button, FoldoutGroup("Animation Testing")]
        public void TestTurnOff() => _animationController?.PlayTurnOff();
        
        [Button, FoldoutGroup("Animation Testing")]
        public void TestMove() => _animationController?.PlayMove();
        
        [Button, FoldoutGroup("Animation Testing")]
        public void TestIdle() => _animationController?.PlayIdle();
        
        [Button, FoldoutGroup("Debug Controls")]
        public void SimulateDamage() {
            _statService.SetStat(EntityStats.CurrentHealth, 0f);
            Debug.Log("Simulated zero health damage");
        }
        
        [Button, FoldoutGroup("Debug Controls")]
        public void RestoreHealth() {
            _statService.SetStat(EntityStats.CurrentHealth, _statService.GetStat(EntityStats.MaxHealth));
            Debug.Log("Restored full health");
        }
        
        [Button, FoldoutGroup("Debug Controls")]
        public void TestMovementCheck() {
            // Debug.Log($"IsMoving: {_enemyMovement?.IsMoving()}");
            Debug.Log($"Current Animation: {_animationController?.GetCurrentState()}");
            Debug.Log($"Turret Active: {_isActive}");
            Debug.Log($"Is Reloading: {_isReloading}");
        }
    }
}