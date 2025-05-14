using _Project.Features.Enemy.BehaviourTree;
using UnityEngine;

namespace _Project.Features.EnemiesModule.Scripts.Enemies.Core {
    public class WaitStrategy : IStrategy {
        private readonly float _totalWaitTime;
        private float _startWaitTime;

        public WaitStrategy(float totalWaitTime) =>
            _totalWaitTime = totalWaitTime;

        public Node.Status Process() {
            if (Time.time - _startWaitTime > _totalWaitTime) {
                Reset();
                return Node.Status.Success;
            }

            return Node.Status.Running;
        }

        public void Reset() =>
            _startWaitTime = Time.time;
    }
}