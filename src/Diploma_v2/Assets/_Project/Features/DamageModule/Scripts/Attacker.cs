using System.Collections.Generic;
using System.Linq;
using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Features.DamageModule {
    public class Attacker : MonoBehaviour, IDamageDealer {
        [SerializeField] private HitBox[] _hitBoxes;
        [SerializeField] private LayerMask _enemyLayerMask;

        [Button]
        public void DealDamage(float damage) {
            IEnumerable<IDamageable> damageables = _hitBoxes.SelectMany(el => el.GetOverlappingDamageables(_enemyLayerMask));
            foreach (IDamageable damageable in damageables.Distinct())
                damageable.TakeDamage(damage);
        }
    }
}