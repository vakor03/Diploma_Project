using _Project.Features.PlayerModule;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Features.EnemyModule.BasicBehaviour
{
    public class EnemyMovement : MonoBehaviour, IMover
    {
        public EnemyMovementData Data;

        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private float _reachDistance = 0.1f;

        public bool IsFacingRight { get; private set; }
        public float LastOnGroundTime { get; private set; }
        public Vector2 MoveDirection { get; set; }
        
        private Vector2Int _currentTargetPosition;
        private bool _hasTarget = false;

        private void Start()
        {
            IsFacingRight = true;
        }

        private void Update()
        {
            LastOnGroundTime -= Time.deltaTime;
            
            UpdateFacingDirection();
            UpdateGroundedState();
            
            // Update movement to target if we have one
            if (_hasTarget)
            {
                Vector3 targetPosition = new Vector3(_currentTargetPosition.x, _currentTargetPosition.y, 0);
                MoveTo(targetPosition);
                
                // Stop moving if we've reached the target
                if (HasReachedPosition(_currentTargetPosition))
                {
                    Stop();
                }
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void UpdateGroundedState()
        {
            if (_groundChecker.CheckGrounded())
            {
                LastOnGroundTime = Data.coyoteTime;
            }
        }

        private void UpdateFacingDirection()
        {
            if (MoveDirection.x != 0)
                CheckDirectionToFace(MoveDirection.x > 0);
        }

        private void Move()
        {
            float targetSpeed = MoveDirection.x * Data.moveMaxSpeed;
            float accelRate;

            if (LastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.moveAccelAmount : Data.moveDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f)
                    ? Data.moveAccelAmount * Data.accelInAir
                    : Data.moveDeccelAmount * Data.deccelInAir;

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

        public void SetMoveDirection(Vector2 direction)
        {
            MoveDirection = direction;
        }

        [Button]
        public void MoveTo(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            SetMoveDirection(new Vector2(direction.x, 0)); // Only use horizontal component
        }

        [Button]
        public void Stop()
        {
            SetMoveDirection(Vector2.zero);
            _hasTarget = false;
        }
        
        #region IMover Implementation
        
        public void MoveTo(Vector2Int position)
        {
            _currentTargetPosition = position;
            _hasTarget = true;
            Vector3 targetPosition = new Vector3(position.x, position.y, 0);
            MoveTo(targetPosition);
        }
        
        public void LookAt(Vector2Int position)
        {
            // For a 2D game, we only care about the horizontal direction
            Vector3 currentPos = transform.position;
            bool shouldFaceRight = position.x > currentPos.x;
            CheckDirectionToFace(shouldFaceRight);
        }
        
        public bool HasReachedPosition(Vector2Int position)
        {
            Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 targetPos = new Vector2(position.x, position.y);
            
            // Only check horizontal distance for 2D platformer
            float horizontalDistance = Mathf.Abs(currentPos.x - targetPos.x);
            
            // Consider it reached if we're close enough in the horizontal axis
            // and we're on the ground (to avoid considering "reaching" while falling)
            return horizontalDistance < _reachDistance && LastOnGroundTime > 0;
        }
        
        #endregion
    }
}