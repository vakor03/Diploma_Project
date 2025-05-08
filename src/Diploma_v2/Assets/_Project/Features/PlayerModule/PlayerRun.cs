using _Project.Features.InputModule;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Features.PlayerModule
{
    public class PlayerRun : MonoBehaviour
    {
        public PlayerRunData Data;

        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private GroundChecker _groundChecker;

        [Inject] private IInputService _inputService;

        public bool IsFacingRight { get; private set; }
        public float LastOnGroundTime { get; private set; }

        private Vector2 _moveInput;

        private void Start()
        {
            IsFacingRight = true;
        }

        private void Update()
        {
            LastOnGroundTime -= Time.deltaTime;

            _moveInput = _inputService.GetMoveDirection();

            UpdateFacingDirection();
            UpdateGroundedState();
        }

        private void FixedUpdate()
        {
            Run();
        }

        private void UpdateGroundedState()
        {
            if (_groundChecker.CheckGrounded())
                LastOnGroundTime = 0.1f;
        }

        private void UpdateFacingDirection()
        {
            if (_moveInput.x != 0)
                CheckDirectionToFace(_moveInput.x > 0);
        }

        private void Run()
        {
            float targetSpeed = _moveInput.x * Data.runMaxSpeed;

            float accelRate;

            if (LastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f)
                    ? Data.runAccelAmount * Data.accelInAir
                    : Data.runDeccelAmount * Data.deccelInAir;

            //Not used since no jump implemented here, but may be useful if you plan to implement your own
            /*
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        */

            //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
            if (Data.doConserveMomentum && Mathf.Abs(_rigidbody2D.linearVelocity.x) > Mathf.Abs(targetSpeed) &&
                Mathf.Sign(_rigidbody2D.linearVelocity.x) == Mathf.Sign(targetSpeed) &&
                Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
            {
                //Prevent any deceleration from happening, or in other words conserve are current momentum
                //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
                accelRate = 0;
            }

            //Calculate difference between current velocity and desired velocity
            float speedDif = targetSpeed - _rigidbody2D.linearVelocity.x;
            //Calculate force along x-axis to apply to thr player

            float movement = speedDif * accelRate;

            //Convert this to a vector and apply to rigidbody
            _rigidbody2D.AddForce(movement * Vector2.right, ForceMode2D.Force);

            /*
             * For those interested here is what AddForce() will do
             * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
             * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
             */
        }

        private void Turn()
        {
            //stores scale and flips the player along the x axis, 
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            IsFacingRight = !IsFacingRight;
        }


        public void CheckDirectionToFace(bool isMovingRight)
        {
            if (isMovingRight != IsFacingRight)
                Turn();
        }
    }
}