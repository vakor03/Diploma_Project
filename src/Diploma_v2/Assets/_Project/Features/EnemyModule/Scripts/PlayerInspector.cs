using _Project.Features.EnemyModule.Blackboard;
using _Project.Features.PlayerSpawnerModule;
using _Project.Features.StatsModule;
using Features.WeaponsModule.Scripts.Weapons.WeaponsInstances;
using UnityEngine;
using Zenject;

namespace _Project.Features.EnemyModule {
    public class PlayerInspector : MonoBehaviour {
        [SerializeField] private EnemyAI _shadowOfStormsAI;
        [SerializeField] private Transform _rotationRoot;

        private LayersConfiguration _layersConfiguration;
        private IStatService<EntityStats> _statService;
        private PlayerTransformDataHolder _playerTransformDataHolder;

        private Blackboard.Blackboard _blackboard;

        public bool IsPlayerInRange { get; private set; }
        public Vector3 LastKnownPlayerPosition { get; private set; }

        private BlackboardKey _lastKnownPlayerPositionKey;
        private BlackboardKey _isPlayerInRangeKey;

        [Inject]
        private void InjectDependencies(LayersConfiguration layersConfiguration, IStatService<EntityStats> statService,
                                        PlayerTransformDataHolder playerTransformDataHolder) {
            _layersConfiguration = layersConfiguration;
            _statService = statService;
            _playerTransformDataHolder = playerTransformDataHolder;
        }

        private void Awake() {
            InitializeComponents();
        }

        private void Update() =>
            DetectPlayerInCone();

        private void InitializeComponents() {
            _blackboard = _shadowOfStormsAI.Blackboard;
            _lastKnownPlayerPositionKey = _blackboard.GetOrRegisterKey(BlackboardKeys.Vector3.LAST_KNOWN_PLAYER_POSITION);
            _isPlayerInRangeKey = _blackboard.GetOrRegisterKey(BlackboardKeys.Bool.IS_PLAYER_IN_RANGE);
        }

        private void DetectPlayerInCone() {
            bool playerDetected = IsPlayerInCone();
            UpdateDetectionStatus(playerDetected);
        }

        private Vector2 GetOrigin() =>
            transform.position;

        private Vector2 GetForwardDirection() =>
            _rotationRoot.right * _rotationRoot.localScale.x;

        private float CalculateStartAngle(Vector2 forward) {
            float halfAngle = _statService[EntityStats.DetectionConeAngle] * 0.5f;
            return Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg - halfAngle;
        }

        private void UpdateDetectionStatus(bool playerDetected) {
            IsPlayerInRange = playerDetected;

            UpdateBlackboard();
        }

        private void UpdateBlackboard() {
            _blackboard.SetValue(_isPlayerInRangeKey, IsPlayerInRange);
            _blackboard.SetValue(_lastKnownPlayerPositionKey, LastKnownPlayerPosition);
        }

        private bool IsPlayerInCone() {
            if (_playerTransformDataHolder.Player == null)
                return false;

            if (!CheckIsPlayerInRange())
                return false;
            if (!IsPlayerInConeAngle())
                return false;
            if (!HasLineOfSight())
                return false;

            LastKnownPlayerPosition = _playerTransformDataHolder.Player.position;
            return true;
        }

        private bool CheckIsPlayerInRange() {
            Vector2 toPlayer = GetVectorToPlayer();
            return toPlayer.magnitude <= _statService[EntityStats.DetectionRange];
        }

        private bool IsPlayerInConeAngle() {
            Vector2 forward = GetForwardDirection();
            Vector2 toPlayer = GetVectorToPlayer();
            float angleToPlayer = Vector2.Angle(forward, toPlayer);

            return angleToPlayer <= _statService[EntityStats.DetectionConeAngle] * 0.5f;
        }

        private bool HasLineOfSight() {
            Vector2 origin = GetOrigin();
            Vector2 toPlayer = GetVectorToPlayer();

            RaycastHit2D hit = Physics2D.Raycast(origin, toPlayer.normalized, toPlayer.magnitude, _layersConfiguration.EnvironmentLayerMask);
            return hit.collider == null;
        }

        private Vector2 GetVectorToPlayer() {
            Vector2 origin = GetOrigin();
            Vector2 playerPosition = _playerTransformDataHolder.Player.position;
            return playerPosition - origin;
        }

        private void DrawCircle(Vector2 center, float radius, int segments) {
            float angleStep = 360f / segments;
            Vector2 prevPoint = center + new Vector2(radius, 0);

            for (int i = 1; i <= segments; i++) {
                float angle = angleStep * i * Mathf.Deg2Rad;
                Vector2 newPoint = center + new Vector2(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius
                );
                Gizmos.DrawLine(prevPoint, newPoint);
                prevPoint = newPoint;
            }
        }

        private void DrawConeBoundaries() {
            Vector2 origin = GetOrigin();
            Vector2 forward = GetForwardDirection();
            float halfAngle = _statService[EntityStats.DetectionConeAngle] * 0.5f;

            Vector2 leftBoundary = RotateVector(forward, -halfAngle);
            Vector2 rightBoundary = RotateVector(forward, halfAngle);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(origin, origin + leftBoundary * _statService[EntityStats.DetectionRange]);
            Gizmos.DrawLine(origin, origin + rightBoundary * _statService[EntityStats.DetectionRange]);
        }

        private void DrawConeArc() {
            Vector2 origin = GetOrigin();
            Vector2 forward = GetForwardDirection();
            float startAngle = CalculateStartAngle(forward);

            Gizmos.color = Color.cyan;
            for (int i = 0; i < 19; i++) {
                Vector2 point1 = CalculateArcPoint(origin, startAngle, i);
                Vector2 point2 = CalculateArcPoint(origin, startAngle, i + 1);
                Gizmos.DrawLine(point1, point2);
            }
        }

        private Vector2 CalculateArcPoint(Vector2 origin, float startAngle, int index) {
            float angle = startAngle + (_statService[EntityStats.DetectionConeAngle] / 19f) * index;
            return origin + new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ) * _statService[EntityStats.DetectionRange];
        }

        private Vector2 RotateVector(Vector2 vector, float angle) =>
            Quaternion.AngleAxis(angle, Vector3.forward) * vector;

        private void OnDrawGizmos() {
            if (!Application.isPlaying)
                return;

            if (IsPlayerInRange) {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(LastKnownPlayerPosition, 0.5f);
            }
            else {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(LastKnownPlayerPosition, 0.5f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(GetOrigin(), LastKnownPlayerPosition);
        }

        private void OnDrawGizmosSelected() {
            if (!Application.isPlaying)
                return;

            DrawRangeCircle();
            DrawConeBoundaries();
            DrawConeArc();
        }

        private void DrawRangeCircle() {
            Gizmos.color = Color.yellow;
            DrawCircle(GetOrigin(), _statService[EntityStats.DetectionRange], 32);
        }
    }
}