using UnityEngine;

namespace _Project.Features.EnemiesModule.Scripts.Enemies.Core {
    public interface IPatrolArea {
        public Transform[] PatrolPoints { get; }
    }
}