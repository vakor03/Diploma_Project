using _Project.Features.EnemyModule._Project.Features.EnemyModule;
using _Project.Features.PlayerModule;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Features.EnemyModule
{
    public class EnemyMovement : MonoBehaviour
    {
        public EnemyMovementData Data;

        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private GroundChecker _groundChecker;

        public bool IsFacingRight { get; private set; }
        public float LastOnGroundTime { get; private set; }
        public Vector2 MoveDirection { get; set; }

        private void Start()
        {
            IsFacingRight = true;
        }

        private void Update()
        {
            LastOnGroundTime -= Time.deltaTime;
            
            UpdateFacingDirection();
            UpdateGroundedState();
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
        }
    }
}