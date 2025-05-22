using System;
using _Project.Features.EnemyModule.BehaviourTrees;
using _Project.Features.EnemyModule.Blackboard;
using _Project.Features.WeaponModule;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Features.EnemyModule {
    public class OnePlaceGuardRobotAI : EnemyAI {
        [Inject] private EntityWeaponDataHolder _weaponDataHolder;
        [SerializeField] private float _turretDeactivationDelay = 5f;
        [SerializeField] private float _timeScaleModifier = 10;

        private bool _fireWeapons;
        private BlackboardKey _playerInRangeKey;
        private BlackboardKey _turretActiveKey;
        private BlackboardKey _noPlayerInRangeTimerKey;

        protected override void SetupTreeComponents(BehaviourTree behaviourTree) {
            Node rootNode = new Selector("Root");
            
            Sequence playerInRangeSequence = new Sequence("PlayerInRangeSequence");
            playerInRangeSequence.AddChild(new Leaf("CheckPlayerInRange", new Condition(() => 
                Blackboard.TryGetValue(_playerInRangeKey, out bool inRange) && inRange)));
            playerInRangeSequence.AddChild(new Leaf("ResetTimer", new ActionStrategy(() => 
                Blackboard.SetValue(_noPlayerInRangeTimerKey, 0f))));
            playerInRangeSequence.AddChild(new Leaf("CheckTurretActive", new Condition(() => 
                Blackboard.TryGetValue(_turretActiveKey, out bool isActive) && isActive)));
            playerInRangeSequence.AddChild(new Leaf("SetFireWeapons", new ActionStrategy(() => _fireWeapons = true)));
            
            Sequence playerNotInRangeSequence = new Sequence("PlayerNotInRangeSequence");
            playerNotInRangeSequence.AddChild(new Leaf("CheckPlayerNotInRange", new Condition(() => 
                Blackboard.TryGetValue(_playerInRangeKey, out bool inRange) && !inRange)));
            playerNotInRangeSequence.AddChild(new Leaf("UpdateTimer", new ActionStrategy(() => {
                if (Blackboard.TryGetValue(_noPlayerInRangeTimerKey, out float currentTime))
                    Blackboard.SetValue(_noPlayerInRangeTimerKey, currentTime + Time.deltaTime * _timeScaleModifier);
            })));
            playerNotInRangeSequence.AddChild(new Leaf("SetFireWeaponsFalse", new ActionStrategy(() => _fireWeapons = false)));
            playerNotInRangeSequence.AddChild(new Leaf("CheckTimer", new Condition(() => {
                if (Blackboard.TryGetValue(_noPlayerInRangeTimerKey, out float currentTime)) {
                    return currentTime >= _turretDeactivationDelay;
                }
                return false;
            })));
            playerNotInRangeSequence.AddChild(new Leaf("DeactivateTurret", new ActionStrategy(() => {
                SwitchTurretActive(false);
                Blackboard.SetValue(_noPlayerInRangeTimerKey, 0f);
            })));
            
            Sequence turretInactiveSequence = new Sequence("TurretInactiveSequence");
            turretInactiveSequence.AddChild(new Leaf("CheckTurretInactive", new Condition(() => 
                Blackboard.TryGetValue(_turretActiveKey, out bool isActive) && !isActive)));
            turretInactiveSequence.AddChild(new Leaf("SetFireWeaponsFalse", new ActionStrategy(() => _fireWeapons = false)));
            
            rootNode.AddChild(turretInactiveSequence);
            rootNode.AddChild(playerInRangeSequence);
            rootNode.AddChild(playerNotInRangeSequence);

            behaviourTree.AddChild(rootNode);
            
            _playerInRangeKey = Blackboard.GetOrRegisterKey(BlackboardKeys.Bool.IS_PLAYER_IN_RANGE);
            _turretActiveKey = Blackboard.GetOrRegisterKey(BlackboardKeys.Bool.IS_TURRET_ACTIVE);
            _noPlayerInRangeTimerKey = Blackboard.GetOrRegisterKey(BlackboardKeys.Float.NO_PLAYER_IN_RANGE_TIMER);
            
            Blackboard.SetValue(_turretActiveKey, false);
            Blackboard.SetValue(_noPlayerInRangeTimerKey, 0f);
        }

        protected override void Update() {
            base.Update();
            if (_fireWeapons)
                FireWeapons();
        }

        private void OnDrawGizmos() {
            if (!Application.isPlaying) {
                return;
            }

            Blackboard.TryGetValue(_playerInRangeKey, out bool inRange);
            Gizmos.color = inRange ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }

        [Button]
        public bool SwitchTurretActive(bool isActive) {
            if (Blackboard.TryGetValue(_turretActiveKey, out bool turretActive)) {
                if (turretActive == isActive)
                    return false;
                
                Blackboard.SetValue(_turretActiveKey, isActive);
                return true;
            }

            Blackboard.SetValue(_turretActiveKey, isActive);
            return true;
        }

        private void FireWeapons()
        {
            foreach (IWeapon weapon in _weaponDataHolder.EquippedWeapons)
                switch (weapon) {
                    case IShootable shootable:
                        shootable.Shoot();
                        break;
                }
        }
    }
}