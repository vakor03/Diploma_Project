using System.Linq;
using _Project.Features.InputModule;
using _Project.Features.PhysicsModule;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Features.PlayerModule {
    public class DropThroughPlatformController : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] private Collider2D _platformCollider;
        [SerializeField] private CollisionHandler2D _platformDetector;

        [Inject] private IInputService _inputService;

        private bool _isDropping = false;

        private void Update() {
            HandleDropThroughInput();
            if (_isDropping)
                HandleIsDropping();
            else
                HandleIsNotDropping();
        }

        private void HandleIsNotDropping() {
            if (ColliderCanBeEnabledBack())
                _platformCollider.enabled = true;
        }

        private void HandleIsDropping() =>
            _platformCollider.enabled = false;

        private bool ColliderCanBeEnabledBack() =>
            !_platformDetector.TriggeredColliders.Any();

        private void HandleDropThroughInput() =>
            _isDropping = _inputService.GetMoveDirection().y < 0;

        [Button]
        void TryDropThrough() {
            _isDropping = true;
            _platformCollider.enabled = false;
        }
    }
}