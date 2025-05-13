using System;
using UnityEngine;

namespace _Project.Features.DamageModule {
    public class EnemyAnimationEvents : MonoBehaviour {
        public event Action AttackEnd;
        public event Action DamageDealt;
        public event Action DamagedAnimationEnd;

        public void OnDamageDealt() =>
            DamageDealt?.Invoke();

        public void OnAttackEnd() =>
            AttackEnd?.Invoke();
        
        public void OnAnimationEnd() =>
            DamagedAnimationEnd?.Invoke();
    }
}