using UnityEngine;

namespace _Project.Features.EnemiesModule.Scripts.Enemies.Core {
    public class PatrolArea : MonoBehaviour, IPatrolArea {
        [field: SerializeField] public Transform[] PatrolPoints { get; private set; }
    }
}