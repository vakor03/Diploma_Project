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
        public float LastPressedJumpTime { get; private set; }

        private Vector2 _moveInput;
        private bool _isJumping;
        private bool _isJumpCut;
        private bool _isJumpFalling;
        private int _airJumpCount; // Track air jumps used
        private bool _hasJumpedThisPress; // Track if we've already jumped for this button press

        private void Start()
        {
            IsFacingRight = true;
        }

        private void Update()
        {
            LastOnGroundTime -= Time.deltaTime;
            LastPressedJumpTime -= Time.deltaTime;

            _moveInput = _inputService.GetMoveDirection();

            // Jump Input - only register press when we haven't jumped yet for this press
            if (_inputService.GetJumpPressed())
            {
                LastPressedJumpTime = Data.jumpBufferTime;
                _hasJumpedThisPress = false; // Reset for new jump press
            }

            // Jump Cut (when button is released early)
            if (_inputService.GetJumpReleased() && _isJumping && _rigidbody2D.linearVelocity.y > 0)
                OnJumpCut();

            UpdateFacingDirection();
            UpdateGroundedState();
            CheckJump();
            UpdateJumpVariables();
            HandleGravity();
        }

        private void FixedUpdate()
        {
            Run();
        }

        private void UpdateGroundedState()
        {
            bool wasGrounded = LastOnGroundTime > 0;
            bool isGrounded = _groundChecker.CheckGrounded();
            
            if (isGrounded)
            {
                // Only reset air jump count when we land after being in the air
                if (!wasGrounded)
                {
                    _airJumpCount = 0;
                    _isJumping = false;
                    _isJumpFalling = false;
                }
                
                LastOnGroundTime = Data.coyoteTime;
            }
        }

        private void UpdateFacingDirection()
        {
            if (_moveInput.x != 0)
                CheckDirectionToFace(_moveInput.x > 0);
        }

        private void UpdateJumpVariables()
        {
            // Check if we're falling after a jump
            if (_isJumping && _rigidbody2D.linearVelocity.y < 0)
            {
                _isJumping = false;
                _isJumpFalling = true;
            }
        }

        private void CheckJump()
        {
            // Only try to jump if we've pressed jump recently and haven't already jumped for this press
            if (LastPressedJumpTime > 0 && !_hasJumpedThisPress)
            {
                // Can we do a regular ground jump?
                if (LastOnGroundTime > 0)
                {
                    GroundJump();
                    return;
                }
                
                // Can we do an air jump?
                if (_airJumpCount < Data.maxAirJumps)
                {
                    AirJump();
                    return;
                }
            }
        }

        private void GroundJump()
        {
            // Reset y velocity and apply jump force
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, 0);
            _rigidbody2D.AddForce(Vector2.up * Data.jumpForce, ForceMode2D.Impulse);

            // Reset jump variables
            _hasJumpedThisPress = true;
            _isJumping = true;
            _isJumpCut = false;
            _isJumpFalling = false;
            _airJumpCount = 0; // Reset air jumps when doing a ground jump
            
            // Debug.Log("Ground jump");
        }

        private void AirJump()
        {
            // Reset y velocity and apply jump force (maybe slightly lower force for air jumps)
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, 0);
            _rigidbody2D.AddForce(Vector2.up * Data.jumpForce, ForceMode2D.Impulse);

            // Reset jump variables
            _hasJumpedThisPress = true;
            _isJumping = true;
            _isJumpCut = false;
            _isJumpFalling = false;
            _airJumpCount++; // Increment air jump count
            
            // Debug.Log($"Air jump #{_airJumpCount}");
        }

        private void OnJumpCut()
        {
            // Apply a downward force when jump button is released early
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, _rigidbody2D.linearVelocity.y * Data.jumpCutMultiplier);
            _isJumpCut = true;
        }

        private void HandleGravity()
        {
            // Handle variable gravity based on state
            float gravityScale = 1f;

            if (_rigidbody2D.linearVelocity.y < 0)
            {
                // Apply stronger gravity when falling
                gravityScale = Data.fallMultiplier;

                // Apply even stronger gravity when pressing down during fall
                if (_moveInput.y < 0)
                    gravityScale = Data.fastFallMultiplier;
            }
            else if (_rigidbody2D.linearVelocity.y > 0 && !_inputService.GetJumpHeld())
            {
                // Apply jump cut gravity when jump button is not held
                gravityScale = Data.jumpCutMultiplier;
            }

            _rigidbody2D.gravityScale = gravityScale;
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

            // Apply higher acceleration and max speed at the apex of jump (hang time)
            if ((_isJumping || _isJumpFalling) && Mathf.Abs(_rigidbody2D.linearVelocity.y) < Data.jumpHangTimeThreshold)
            {
                accelRate *= Data.jumpHangAccelerationMult;
                targetSpeed *= Data.jumpHangMaxSpeedMult;
            }

            // Conserve momentum
            if (Data.doConserveMomentum && Mathf.Abs(_rigidbody2D.linearVelocity.x) > Mathf.Abs(targetSpeed) &&
                Mathf.Sign(_rigidbody2D.linearVelocity.x) == Mathf.Sign(targetSpeed) &&
                Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
            {
                accelRate = 0;
            }

            float speedDif = targetSpeed - _rigidbody2D.linearVelocity.x;
            float movement = speedDif * accelRate;

            _rigidbody2D.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        private void Turn()
        {
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