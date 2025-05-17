using System;
using System.Collections.Generic;

namespace _Project.Features.EnemyModule.BehaviourTrees {
    public class BehaviourTree : Node, IStrategy {
        public BehaviourTree(string name = "Behaviour Tree") : base(name) { }

        public override Status Process() {
            while (_currentChild < Children.Count) {
                Status status = Children[_currentChild].Process();
                if (status != Status.Success) {
                    return status;
                }
                _currentChild++;
            }
            return Status.Failure;
        }
    }

    public class Selector : Node {
        public Selector(string name = "Selector") : base(name) { }

        public override Status Process() {
            if (_currentChild < Children.Count) {
                Status status = Children[_currentChild].Process();
                switch (status) {
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    case Status.Failure: 
                        _currentChild++;
                        return Status.Running;
                    case Status.Running:
                        return Status.Running;
                    default: throw new ArgumentOutOfRangeException();
                }
                
            }
            
            Reset();
            return Status.Failure;
        }
    }

    public class Sequence : Node {
        public Sequence(string name = "Sequence") : base(name) { }

        public override Status Process() {
            if (_currentChild < Children.Count) {
                switch (Children[_currentChild].Process()) {
                    case Status.Success:
                        _currentChild++;
                        return _currentChild ==  Children.Count ? Status.Success : Status.Running;
                    case Status.Failure:
                        Reset();
                        return Status.Failure;
                    case Status.Running:
                        return Status.Running;
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            Reset();
            return Status.Success;
        }
    }

    public class Leaf : Node {
        private readonly IStrategy _strategy;

        public Leaf(IStrategy strategy, string name) : base(name) =>
            _strategy = strategy;

        public override Status Process() =>
            _strategy.Process();

        public override void Reset() =>
            _strategy.Reset();
    }

    public class Node {
        public enum Status {
            Success = 0,
            Failure = 1,
            Running = 2,
        }

        public readonly string Name;
        public readonly List<Node> Children = new();

        protected int _currentChild;

        public Node(string name = "Node") =>
            Name = name;
        
        public void AddChild(Node child) => 
            Children.Add(child);

        public virtual Status Process() =>
            Children[_currentChild].Process();
        
        public virtual void Reset() {
            _currentChild = 0;
            foreach (Node child in Children)
                child.Reset();
        }
    }
}