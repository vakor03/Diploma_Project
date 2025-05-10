// PlatformDropThrough.cs

using _Project.Features.InputModule;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class DropThroughPlatformController : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float dropThroughTime = 0.5f;

    [SerializeField] private LayerMask platformLayers = -1;

    [SerializeField] private Collider2D _platformCollider;

    [Inject] private IInputService _inputService;

    private bool _isDropping = false;
    private float _dropTimer = 0f;
    private float _originalRotation = 0f;

    void Update() {
        HandleDropThroughInput();
        UpdateDropState();
    }

    void HandleDropThroughInput() {
        if (_inputService.GetMoveDirection().y < 0) {
            TryDropThrough();
        }
    }

    [Button]
    void TryDropThrough() {
        _isDropping = true;
        _dropTimer = dropThroughTime;
        _platformCollider.enabled = false;
    }

    void UpdateDropState() {
        if (_isDropping) {
            _dropTimer -= Time.deltaTime;

            if (_dropTimer <= 0f) {
                _platformCollider.enabled = true;

                _isDropping = false;
            }
        }
    }
}