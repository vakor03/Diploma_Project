using System;
using System.Collections.Generic;
using _Project.Features.DamageModule;
using _Project.Features.EnemyModule.BasicBehaviour;
using _Project.Features.EnemyModule.BehaviourTrees;
using _Project.Features.InputModule;
using _Project.Features.LevelGeneratorModule;
using _Project.Features.StatsModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.EnemyModule {
    public class ShadowOfStormsAI : EnemyAI {
        private IInputService _inputService;
        private BlockGroupsModel _blockGroupsModel;
        private IBlockGroupService _blockGroupService;
        private IStatService<EntityStats> _statService;
        [SerializeField] private SimpleEnemyAttack _attack1;
        [SerializeField] private SimpleEnemyAttack _attack2;
        [SerializeField] private SimpleEnemyAttack _attack3;
        [SerializeField] private EnemyMovement _enemyMovement;
        

        [Inject]
        private void InjectDependencies(IInputService inputService, BlockGroupsModel blockGroupsModel, IBlockGroupService blockGroupService, IStatService<EntityStats> statService) {
            _inputService = inputService;
            _blockGroupsModel = blockGroupsModel;
            _blockGroupService = blockGroupService;
            _statService = statService;
        }

        protected override void SetupTreeComponents(BehaviourTree behaviourTree) {
            Leaf isUsingAttack1 = new Leaf("IsUsingAttack1", new Condition(IsUsingAttack1));
            Leaf isUsingAttack2 = new Leaf("IsUsingAttack2", new Condition(IsUsingAttack2));
            Leaf isUsingAttack3 = new Leaf("IsUsingAttack3", new Condition(IsUsingAttack3));
            
            Leaf startAttack1 = new Leaf("StartAttack1", new StartAttackStrategy(_attack1));
            Leaf startAttack2 = new Leaf("StartAttack2", new StartAttackStrategy(_attack2));
            Leaf startAttack3 = new Leaf("StartAttack3", new StartAttackStrategy(_attack3));
            
            Leaf waitForAttack1End = new Leaf("WaitForAttack1End", new WaitForAttackEndStrategy(_attack1));
            Leaf waitForAttack2End = new Leaf("WaitForAttack2End", new WaitForAttackEndStrategy(_attack2));
            Leaf waitForAttack3End = new Leaf("WaitForAttack3End", new WaitForAttackEndStrategy(_attack3));

            Leaf stopMovement = new Leaf("Stop Mover", new ActionStrategy(() => _enemyMovement.Stop()));

            Sequence attack1Sequence = new Sequence("Attack1Sequence");
            attack1Sequence.AddChild(isUsingAttack1);
            attack1Sequence.AddChild(stopMovement);
            attack1Sequence.AddChild(startAttack1);
            attack1Sequence.AddChild(waitForAttack1End);
            attack1Sequence.AddChild(new Leaf("LogAttack1End", new ActionStrategy(LogAttack1End)));

            Sequence attack2Sequence = new Sequence("Attack2Sequence");
            attack2Sequence.AddChild(isUsingAttack2);
            attack2Sequence.AddChild(stopMovement);
            attack2Sequence.AddChild(startAttack2);
            attack2Sequence.AddChild(waitForAttack2End);
            attack2Sequence.AddChild(new Leaf("LogAttack2End", new ActionStrategy(LogAttack2End)));

            Sequence attack3Sequence = new Sequence("Attack3Sequence");
            attack3Sequence.AddChild(isUsingAttack3);
            attack3Sequence.AddChild(stopMovement);
            attack3Sequence.AddChild(startAttack3);
            attack3Sequence.AddChild(waitForAttack3End);
            attack3Sequence.AddChild(new Leaf("LogAttack3End", new ActionStrategy(LogAttack3End)));

            PrioritySelector attackSelector = new PrioritySelector("AttackSelector", 10);
            attackSelector.AddChild(attack1Sequence);
            attackSelector.AddChild(attack2Sequence);
            attackSelector.AddChild(attack3Sequence);
            
            Sequence patrolSequence = new Sequence("PatrolSequence");
            patrolSequence.AddChild(new Leaf("Patrol", new PatrolStrategy(_enemyMovement, _patrolPoints, 2f)));
            
            PrioritySelector overallSelector = new PrioritySelector("OverallSelector");
            overallSelector.AddChild(attackSelector);
            overallSelector.AddChild(patrolSequence);
            
            Sequence rootSequence = new Sequence("RootSequence");
            rootSequence.AddChild(new Leaf("IsAlive", new Condition(() => _statService.GetStat(EntityStats.CurrentHealth) > 0)));
            rootSequence.AddChild(overallSelector);

            behaviourTree.AddChild(rootSequence);
        }
        
        private List<Vector2Int> _patrolPoints = new List<Vector2Int>();

        protected override void Update() {
            if (_patrolPoints.Count == 0)
                GetPatrolPoints(3);
            base.Update();
        }

        private void GetPatrolPoints(int patrolPointsCount) {
            BlockGroup group = _blockGroupService.FindGroupContaining(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));
            _patrolPoints.Clear();
            
            if (group != null && group.Blocks.Count > 0) {
                // Find leftmost and rightmost positions
                Vector2Int leftmost = group.Blocks[0];
                Vector2Int rightmost = group.Blocks[0];
                
                foreach (Vector2Int block in group.Blocks) {
                    if (block.x < leftmost.x) leftmost = block;
                    if (block.x > rightmost.x) rightmost = block;
                }
                
                // Add leftmost and rightmost points
                _patrolPoints.Add(leftmost);
                _patrolPoints.Add(rightmost);
            }
        }

        private bool IsUsingAttack1() =>
            _inputService.EnemyDebugAttack1;

        private bool IsUsingAttack2() =>
            _inputService.EnemyDebugAttack2;

        private bool IsUsingAttack3() =>
            _inputService.EnemyDebugAttack3;

        private void LogAttack1Start() {
            // Implement the logic for attack 1
            Debug.Log("Performing Attack 1");
        }

        private void LogAttack2Start() {
            // Implement the logic for attack 2
            Debug.Log("Performing Attack 2");
        }

        private void LogAttack3Start() {
            // Implement the logic for attack 3
            Debug.Log("Performing Attack 3");
        }
        
        private void LogAttack1End() {
            // Implement the logic for attack 1 end
            Debug.Log("Attack 1 Ended");
        }
        
        private void LogAttack2End() {
            // Implement the logic for attack 2 end
            Debug.Log("Attack 2 Ended");
        }
        
        private void LogAttack3End() {
            // Implement the logic for attack 3 end
            Debug.Log("Attack 3 Ended");
        }

        private void OnDrawGizmos() {
            BlockGroup group = _blockGroupService.FindGroupContaining(new Vector2Int((int)transform.position.x, (int)transform.position.y));
            if (group != null) {
                foreach (Vector2Int groupBlock in group.Blocks) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(new Vector3(groupBlock.x, groupBlock.y, 0), Vector3.one);
                }
            }
        }
    }

    public class WaitForAttackEndStrategy : IStrategy {
        private readonly SimpleEnemyAttack _attack;
        public WaitForAttackEndStrategy(SimpleEnemyAttack attack) =>
            _attack = attack;

        public Node.Status Process() =>
            _attack.IsAttacking ? Node.Status.Running : Node.Status.Success;
    }
    
    public class StartAttackStrategy : IStrategy {
        private readonly SimpleEnemyAttack _attack;
        public StartAttackStrategy(SimpleEnemyAttack attack) =>
            _attack = attack;

        public Node.Status Process() {
            _attack.PerformAttack();
            return Node.Status.Success;
        }
    }
    
    public interface IMover
    {
        void MoveTo(Vector2Int position);
        void LookAt(Vector2Int position);
        bool HasReachedPosition(Vector2Int position);
    }

    public class PatrolStrategy : IStrategy
    {
        readonly IMover mover;
        readonly List<Vector2Int> patrolPoints;
        readonly float patrolSpeed;
        int currentIndex;
        bool movingRight = true;

        public PatrolStrategy(IMover mover, List<Vector2Int> patrolPoints, float patrolSpeed = 2f)
        {
            this.mover = mover;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
        }

        public Node.Status Process()
        {
            if (patrolPoints.Count < 2)
                return Node.Status.Failure;
        
            Vector2Int targetPosition = movingRight ? patrolPoints[1] : patrolPoints[0];
            mover.MoveTo(targetPosition);
            mover.LookAt(targetPosition);
        
            if (mover.HasReachedPosition(targetPosition))
            {
                movingRight = !movingRight;
            }
        
            return Node.Status.Running;
        }
    
        public void Reset()
        {
            movingRight = true;
        }
    }
}