using UnityEngine;

namespace _Project.Features.EnemyModule.Enemies.Core {
    public interface IPatrolArea {
        public Transform[] PatrolPoints { get; }
    }
}