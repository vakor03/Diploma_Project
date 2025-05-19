using UnityEngine;

namespace _Project.Features.EnemyModule.BasicBehaviour {
    [CreateAssetMenu(fileName = "EnemyMovementData", menuName = "EnemyModule/Movement Data")] 
    public class EnemyMovementData : ScriptableObject
    {
        [Header("Movement")]
        public float moveAcceleration = 2f;
        public float moveDecceleration = 2f;
        
        [Header("Air Movement")]
        [Range(0.01f, 1)] public float accelInAir = 0.8f;
        [Range(0.01f, 1)] public float deccelInAir = 0.8f;
        
        [Header("Ground Detection")]
        public float coyoteTime = 0.1f;

        public float CalculateMoveAccelAmount(float currentSpeed)
        {
            return (50 * moveAcceleration) / currentSpeed;
        }

        public float CalculateMoveDeccelAmount(float currentSpeed)
        {
            return (50 * moveDecceleration) / currentSpeed;
        }
    }
}