using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public class EnemyPickup : PickupItem {
        protected override bool CanPickUp(Collider2D other) =>
            other.gameObject.TryGetComponent<Enemy>(out _);
    }
}