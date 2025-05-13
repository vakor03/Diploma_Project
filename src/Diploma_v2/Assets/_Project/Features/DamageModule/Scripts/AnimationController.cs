using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Features.DamageModule {
    public class AnimationController : MonoBehaviour {
        [SerializeField] private EnemyAnimatorStateMachine _enemyAnimatorStateMachine;

        [SerializeField] private MonoMovable _movable;
        [SerializeField] private EnemyAnimationEvents _animationEvents;

        [SerializeField]private bool _isAttacking;
        [SerializeField]private bool _isDead;
        [SerializeField]private bool _isStatic;
        [SerializeField]private float _currentSpeed;
        [SerializeField] private bool _isDamaged;

        private void OnEnable() {
            _animationEvents.DamagedAnimationEnd += OnDamagedAnimationEnd;
        }
        
        private void OnDisable() {
            _animationEvents.DamagedAnimationEnd -= OnDamagedAnimationEnd;
        }

        private void OnDamagedAnimationEnd() {
            _isDamaged = false;
            _enemyAnimatorStateMachine.SwitchToState(ChooseMovementStateBasedOnSpeed());
        }


        private void Update() {
            // _currentSpeed = _movable.CurrentSpeed;

            if (_isAttacking || _isDead || _isDamaged)
                return;

            _enemyAnimatorStateMachine.SwitchToState(ChooseMovementStateBasedOnSpeed());
        }

        private State ChooseMovementStateBasedOnSpeed() {
            if (_currentSpeed > 0)
                return State.Run;

            if (_isStatic)
                return State.IdleStatic;

            return State.Idle;
        }

        [Button]
        public void StartAttackWithState(State attackState) {
            _isAttacking = true;
            _enemyAnimatorStateMachine.SwitchToState(attackState);
        }
        [Button]

        public void StopAttack() {
            if (!_isAttacking)
                return;

            _isAttacking = false;
        }

        [Button]
        public void SetIsDamaged() {
            _isDamaged = true;
            _enemyAnimatorStateMachine.SwitchToState(State.Damaged);
        }

        [Button]

        public void SetIsDead() {
            _isDead = true;
            _enemyAnimatorStateMachine.SwitchToState(State.Death);
        }
        [Button]

        public void Reset() {
            _isDead = false;
            _isAttacking = false;
            _isDamaged = false;
        }
    }
}