using System;
using _Project.Features.EnemyModule.BasicBehaviour;
using UnityEngine;

namespace _Project.Features.EnemyModule.BehaviourTrees {
    public interface IStrategy {
        public Node.Status Process();

        public void Reset() {
            // No-op
        }
    }

    public class Condition : IStrategy {
        private readonly Func<bool> _predicate;

        public Condition(Func<bool> predicate) =>
            _predicate = predicate;

        public Node.Status Process() =>
            _predicate() ? Node.Status.Success : Node.Status.Failure;
    }

    public class ActionStrategy : IStrategy {
        private readonly Action _action;

        public ActionStrategy(Action action) =>
            _action = action;

        public Node.Status Process() {
            _action();
            return Node.Status.Success;
        }
    }


    public class PatrolStrategy : IStrategy {
        public Node.Status Process() {
            Debug.LogError("Patrolling");
            return Node.Status.Running;
        }
    }

    public class FollowStrategy : IStrategy {
        private readonly Transform _target;
        private readonly EnemyMovement _enemyMovement;
        
        public FollowStrategy(Transform target, EnemyMovement enemyMovement) {
            _target = target;
            _enemyMovement = enemyMovement;
        }

        public Node.Status Process() {
            _enemyMovement.MoveTo(_target.position);
            return Node.Status.Running;
        }
    }
}