using _Project.Features.InputModule;
using _Project.Features.PlayerModule;
using UnityEngine;
using Zenject;

public class PlayerAnimatorController : MonoBehaviour {
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Animator _animator;
    [SerializeField] private GroundChecker _groundChecker;
    private IInputService _inputService;


    private void Update() {
        _animator.SetFloat(Speed, Mathf.Abs(_inputService.GetMoveDirection().x));
        _animator.SetBool(IsGrounded, _groundChecker.CheckGrounded());
    }

    [Inject]
    private void InjectDependencies(IInputService inputService) {
        _inputService = inputService;
    }
}