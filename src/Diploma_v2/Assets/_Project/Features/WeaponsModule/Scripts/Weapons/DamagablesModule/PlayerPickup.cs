using _Project.Features.PlayerModule;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public class PlayerPickup : PickupItem {
        protected override bool CanPickUp(Collider2D other) =>
            other.gameObject.TryGetComponent<Player>(out _);
    }
}