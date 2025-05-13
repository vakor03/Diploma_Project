using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Features.DamageModule {
    public class SimpleEnemyAttack : MonoBehaviour {
        [SerializeField] private List<float> _damage;
        [SerializeField] private State _attackState;
        [SerializeField] private EnemyAnimationEvents _enemyAnimationEvents;
        [SerializeField] private Attacker _attacker;
        [SerializeField] private AnimationController _animationController;

        public event Action OnAttackStart;
        public event Action OnAttackEnd;
        
        private int _currentDamageIndex = 0;

        [Button]
        public void PerformAttack() {
            OnAttackStart?.Invoke();
            _enemyAnimationEvents.AttackEnd += HandleAttackEndEvent;
            _enemyAnimationEvents.DamageDealt += HandleDamageDealtEvent;
            _currentDamageIndex = 0;
            _animationController.StartAttackWithState(_attackState);
        }

        private void HandleDamageDealtEvent() {
            _attacker.DealDamage(_damage[_currentDamageIndex]);
            _currentDamageIndex++;
        }
        
        private void HandleAttackEndEvent() {
            _animationController.StopAttack();
            _enemyAnimationEvents.AttackEnd -= HandleAttackEndEvent;
            _enemyAnimationEvents.DamageDealt -= HandleDamageDealtEvent;
            OnAttackEnd?.Invoke();
        }
    }
}