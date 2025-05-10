using UnityEngine;

namespace _Project.Features.EnemyModule._Project.Features.EnemyModule {
    [CreateAssetMenu(fileName = "EnemyMovementData", menuName = "Enemy/Movement Data")] 
    public class EnemyMovementData : ScriptableObject
    {
        [Header("Movement")]
        public float moveMaxSpeed = 5f;
        public float moveAcceleration = 2f;
        [HideInInspector] public float moveAccelAmount;
        public float moveDecceleration = 2f;
        [HideInInspector] public float moveDeccelAmount;
        
        [Header("Air Movement")]
        [Range(0.01f, 1)] public float accelInAir = 0.8f;
        [Range(0.01f, 1)] public float deccelInAir = 0.8f;
        
        [Header("Ground Detection")]
        public float coyoteTime = 0.1f;

        private void OnValidate()
        {
            moveAccelAmount = (50 * moveAcceleration) / moveMaxSpeed;
            moveDeccelAmount = (50 * moveDecceleration) / moveMaxSpeed;

            moveAcceleration = Mathf.Clamp(moveAcceleration, 0.01f, moveMaxSpeed);
            moveDecceleration = Mathf.Clamp(moveDecceleration, 0.01f, moveMaxSpeed);
        }
    }
}