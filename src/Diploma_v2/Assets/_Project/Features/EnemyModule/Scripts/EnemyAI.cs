using _Project.Features.EnemyModule.BehaviourTrees;
using UnityEngine;

namespace _Project.Features.EnemyModule {
    public abstract class EnemyAI : MonoBehaviour {
        private BehaviourTree _tree;
        protected Blackboard.Blackboard Blackboard { get; private set; }

        private void Awake() {
            Blackboard = new();
            ConstructBehaviourTree();
        }

        private void ConstructBehaviourTree() {
            _tree = new BehaviourTree("EnemyAI");
            SetupTreeComponents(_tree);
        }

        protected abstract void SetupTreeComponents(BehaviourTree behaviourTree);

        protected virtual void Update() =>
            _tree.Process();
    }
}