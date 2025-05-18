using _Project.Features.DamageModule;
using _Project.Features.EnemyModule.BehaviourTrees;
using _Project.Features.InputModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.EnemyModule {
    public class ShadowOfStormsAI : EnemyAI {
        private IInputService _inputService;
        [SerializeField] private SimpleEnemyAttack _attack1;
        [SerializeField] private SimpleEnemyAttack _attack2;
        [SerializeField] private SimpleEnemyAttack _attack3;

        [Inject]
        private void InjectDependencies(IInputService inputService) =>
            _inputService = inputService;

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

            Sequence attack1Sequence = new Sequence("Attack1Sequence");
            attack1Sequence.AddChild(isUsingAttack1);
            attack1Sequence.AddChild(startAttack1);
            attack1Sequence.AddChild(waitForAttack1End);
            attack1Sequence.AddChild(new Leaf("LogAttack1End", new ActionStrategy(LogAttack1End)));

            Sequence attack2Sequence = new Sequence("Attack2Sequence");
            attack2Sequence.AddChild(isUsingAttack2);
            attack2Sequence.AddChild(startAttack2);
            attack2Sequence.AddChild(waitForAttack2End);
            attack2Sequence.AddChild(new Leaf("LogAttack2End", new ActionStrategy(LogAttack2End)));

            Sequence attack3Sequence = new Sequence("Attack3Sequence");
            attack3Sequence.AddChild(isUsingAttack3);
            attack3Sequence.AddChild(startAttack3);
            attack3Sequence.AddChild(waitForAttack3End);
            attack3Sequence.AddChild(new Leaf("LogAttack3End", new ActionStrategy(LogAttack3End)));

            PrioritySelector attackSelector = new PrioritySelector("AttackSelector");
            attackSelector.AddChild(attack1Sequence);
            attackSelector.AddChild(attack2Sequence);
            attackSelector.AddChild(attack3Sequence);

            behaviourTree.AddChild(attackSelector);

            // Add any additional behaviors like patrolling if needed
            // For now they're commented out
            // Leaf patrolLeaf = new Leaf("Patrol", new PatrolStrategy());
            // behaviourTree.AddChild(patrolLeaf);
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
}