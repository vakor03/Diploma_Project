using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;
using UnityEngine;

namespace _Project.Features.DamageModule {
    public class HurtBox : MonoBehaviour {
        public IDamageable Damageable { get; set; }
    }
}