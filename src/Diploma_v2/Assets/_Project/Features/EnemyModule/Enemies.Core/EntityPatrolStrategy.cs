using _Project.Features.EnemyModule.BehaviourTrees;

namespace _Project.Features.EnemyModule.Enemies.Core {
    public class EntityPatrolStrategy : IStrategy {
        private readonly IPatrolArea _patrolArea;

        private int _currentPatrolPointIndex;

        public EntityPatrolStrategy(IPatrolArea patrolArea) {
            _patrolArea = patrolArea;
        }

        public Node.Status Process() {
            // if (_moveStrategy == null)
            //     return Node.Status.Failure;
            //
            // if (Vector2.Distance(_moveStrategy.Transform.position.With(y: 0), _patrolArea.PatrolPoints[_currentPatrolPointIndex].position.With(y: 0)) < 0.1f)
            //     _currentPatrolPointIndex = _currentPatrolPointIndex + 1;

            if (_currentPatrolPointIndex == _patrolArea.PatrolPoints.Length) {
                Reset();
                return Node.Status.Success;
            }

            // _moveStrategy.Move((_patrolArea.PatrolPoints[_currentPatrolPointIndex].position - _moveStrategy.Transform.position).normalized);

            return Node.Status.Running;
        }

        public void Reset() =>
            _currentPatrolPointIndex = 0;
    }
}