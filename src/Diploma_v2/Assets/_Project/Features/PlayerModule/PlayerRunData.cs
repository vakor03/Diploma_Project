using UnityEngine;

namespace _Project.Features.PlayerModule
{
    [CreateAssetMenu(menuName = "Player Run Data")] 
    public class PlayerRunData : ScriptableObject
    {
        [Header("Run")]
        public float runMaxSpeed = 10f;
        public float runAcceleration = 3f;
        [HideInInspector] public float runAccelAmount;
        public float runDecceleration = 3f;
        [HideInInspector] public float runDeccelAmount;
        [Space(10)]
        [Range(0.01f, 1)] public float accelInAir = 0.65f;
        [Range(0.01f, 1)] public float deccelInAir = 0.65f;
        public bool doConserveMomentum = true;

        [Header("Jump")]
        public float jumpForce = 14f;                    // Base jump force
        public float jumpCutMultiplier = 0.4f;           // Multiplier when jump button is released early
        public float coyoteTime = 0.15f;                 // Time after leaving ground that player can still jump
        public float jumpBufferTime = 0.12f;             // Time before hitting ground that jump input is buffered
        public int maxAirJumps = 1;                      // Number of mid-air jumps allowed (Celeste has 1)
        public float jumpHangTimeThreshold = 1.5f;       // Velocity threshold for jump hang time
        public float jumpHangAccelerationMult = 1.3f;    // Increased acceleration at jump apex
        public float jumpHangMaxSpeedMult = 1.2f;        // Increased max speed at jump apex
        public float fallMultiplier = 1.7f;              // Gravity multiplier when falling
        public float fastFallMultiplier = 2.5f;          // Gravity multiplier when pressing down during fall

        private void OnValidate()
        {
            runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
            runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

            #region Variable Ranges
            runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
            runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
            #endregion
        }
    }
}