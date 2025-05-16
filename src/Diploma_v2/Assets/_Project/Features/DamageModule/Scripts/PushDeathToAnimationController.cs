using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;
using UnityEngine;

namespace _Project.Features.DamageModule {
    public class PushDeathToAnimationController : MonoBehaviour {
        [SerializeField] private MonoDamageable _damageable;
        [SerializeField] private AnimationController _animationController;
        
        private void OnEnable() =>
            _damageable.OnDeath += HandleOnDeath;

        private void OnDisable() =>
            _damageable.OnDeath -= HandleOnDeath;

        private void HandleOnDeath() {
            if (_animationController == null)
                return;

            _animationController.SetIsDead();
        }
    }
}