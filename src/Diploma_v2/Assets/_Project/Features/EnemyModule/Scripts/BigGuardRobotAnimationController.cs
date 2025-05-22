using UnityEngine;

namespace _Project.Features.EnemyModule {
    public class BigGuardRobotAnimationController : MonoBehaviour {
        
        [Header("Animation Settings")]
        [SerializeField] private float _crossfadeDuration = 0.3f;
        [SerializeField] private bool _debugMode = true;
        
        [SerializeField] private Animator _animator;
        private AnimationState _currentState = AnimationState.TurnOff;
        
        // Animation state hashes for performance
        private static readonly int IdleHash = Animator.StringToHash("Move");
        private static readonly int MoveHash = Animator.StringToHash("Run");
        private static readonly int TurnOnHash = Animator.StringToHash("TurnOn");
        private static readonly int TurnOffHash = Animator.StringToHash("TurnOff");
        
        public enum AnimationState {
            Idle,
            Move,
            TurnOn,
            TurnOff
        }
        
        private void Awake() {
            if (_animator == null) {
                _animator = GetComponent<Animator>();
            }
        }
        
        private void Start() {
            // Start with turn on animation
            PlayTurnOn();
        }
        
        /// <summary>
        /// Play the turn on animation
        /// </summary>
        public void PlayTurnOn() {
            ChangeState(AnimationState.TurnOn);
        }
        
        /// <summary>
        /// Play the turn off animation
        /// </summary>
        public void PlayTurnOff() {
            ChangeState(AnimationState.TurnOff);
        }
        
        /// <summary>
        /// Play the move animation
        /// </summary>
        public void PlayMove() {
            ChangeState(AnimationState.Move);
        }
        
        /// <summary>
        /// Play the idle animation
        /// </summary>
        public void PlayIdle() {
            ChangeState(AnimationState.Idle);
        }
        
        /// <summary>
        /// Check if a specific animation is currently playing
        /// </summary>
        public bool IsPlaying(AnimationState state) {
            return _currentState == state;
        }
        
        /// <summary>
        /// Get the current animation state
        /// </summary>
        public AnimationState GetCurrentState() {
            return _currentState;
        }
        
        /// <summary>
        /// Check if the current animation has finished playing
        /// </summary>
        public bool IsCurrentAnimationComplete() {
            if (_animator == null) return true;
            
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.normalizedTime >= 1.0f && !_animator.IsInTransition(0);
        }
        
        private void ChangeState(AnimationState newState) {
            // Don't change if it's the same state
            if (_currentState == newState) {
                if (_debugMode) Debug.Log($"BigGuardRobot: Already in state {newState}, skipping");
                return;
            }
            
            AnimationState previousState = _currentState;
            _currentState = newState;
            
            // Get the animation hash for the new state
            int animationHash = GetAnimationHash(newState);
            
            // Play the animation
            if (_animator != null) {
                _animator.CrossFade(animationHash, _crossfadeDuration);
            }
            
            if (_debugMode) {
                Debug.Log($"BigGuardRobot: State changed from {previousState} to {newState}");
            }
        }
        
        private int GetAnimationHash(AnimationState state) {
            return state switch {
                AnimationState.Idle => IdleHash,
                AnimationState.Move => MoveHash,
                AnimationState.TurnOn => TurnOnHash,
                AnimationState.TurnOff => TurnOffHash,
                _ => IdleHash
            };
        }
        
        #if UNITY_EDITOR
        [Header("Debug Info (Runtime Only)")]
        [SerializeField, Space] private string _currentStateDisplay;
        [SerializeField] private bool _isAnimationComplete;
        
        private void OnValidate() {
            if (Application.isPlaying) {
                _currentStateDisplay = _currentState.ToString();
                _isAnimationComplete = IsCurrentAnimationComplete();
            }
        }
        #endif
        
        // Debug methods for testing
        [UnityEngine.ContextMenu("Test Turn On")]
        public void TestTurnOn() => PlayTurnOn();
        
        [UnityEngine.ContextMenu("Test Turn Off")]
        public void TestTurnOff() => PlayTurnOff();
        
        [UnityEngine.ContextMenu("Test Move")]
        public void TestMove() => PlayMove();
        
        [UnityEngine.ContextMenu("Test Idle")]
        public void TestIdle() => PlayIdle();
    }
}