using System.Collections.Generic;
using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;
using UnityEngine;

namespace _Project.Features.DamageModule {
    public class HitBox : MonoBehaviour {
        [SerializeField] private List<Collider2D> _hitColliders;

        private readonly List<Collider2D> _cache = new List<Collider2D>();

        public List<IDamageable> GetOverlappingDamageables(LayerMask layerMask) {
            List<IDamageable> damageables = new List<IDamageable>();
            ContactFilter2D contactFilter = new ContactFilter2D {
                useTriggers = true,
                useLayerMask = true,
                layerMask = layerMask
            };
            foreach (Collider2D hitCollider in _hitColliders) {
                int collidersOverlapped = Physics2D.OverlapCollider(hitCollider, contactFilter, _cache);
                for (int i = 0; i < collidersOverlapped; i++) {
                    if (!_cache[i].TryGetComponent(out HurtBox hurtBox))
                        continue;

                    if (damageables.Contains(hurtBox.Damageable))
                        continue;

                    damageables.Add(hurtBox.Damageable);
                }
            }

            return damageables;
        }
    }
}