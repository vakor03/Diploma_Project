// PlatformDropThrough.cs

using _Project.Features.InputModule;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class DropThroughPlatformController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float dropThroughTime = 0.5f;
    [SerializeField] private LayerMask platformLayers = -1;
    
    [SerializeField] private Collider2D _platformCollider;
    
    [Inject] private IInputService _inputService;
    
    private bool _isDropping = false;
    private float _dropTimer = 0f;
    // private Collider2D _currentPlatform = null;
    // private PlatformEffector2D _currentEffector = null;
    private float _originalRotation = 0f;
    
    void Update()
    {
        HandleDropThroughInput();
        UpdateDropState();
    }
    
    void HandleDropThroughInput()
    {
        // Check for down + jump input
        if (_inputService.GetMoveDirection().y < 0)
        {
            TryDropThrough();
        }
    }
    
    [Button]
    void TryDropThrough()
    {
        // Find platform directly below
        // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, platformLayers);
        //
        // if (hit.collider != null)
        // {
            // _currentPlatform = hit.collider;
            // _currentEffector = _currentPlatform.GetComponent<PlatformEffector2D>();
            _isDropping = true;
            _dropTimer = dropThroughTime;
            
            // if (_currentEffector != null && _currentEffector.useOneWay)
            {
                _platformCollider.enabled = false;
            }
        // }
    }
    
    void UpdateDropState()
    {
        if (_isDropping)
        {
            _dropTimer -= Time.deltaTime;
            
            if (_dropTimer <= 0f)
            {
                // Restore platform effector
               _platformCollider.enabled = true;
                
                _isDropping = false;
                // _currentPlatform = null;
                // _currentEffector = null;
            }
        }
    }
}