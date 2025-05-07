using System;

namespace _Project.Features.Enemy.BehaviourTree {
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
        
        public Node.Status Process() => _predicate() ?  Node.Status.Success : Node.Status.Failure;
    }
    
    public class ActionStrategy : IStrategy {
        private readonly Action _action;
        public Node.Status Process() {
            _action();
            return Node.Status.Success;
        }
    }
}