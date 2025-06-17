using _Project.Features.PlayerModule;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour {
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Animator _animator;
    [SerializeField] private GroundChecker _groundChecker;


    private void Update() {
        _animator.SetFloat(Speed, Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        _animator.SetBool(IsGrounded, _groundChecker.CheckGrounded());
    }
}