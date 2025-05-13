using UnityEngine;

namespace _Project.Features.DamageModule {
    public class SimpleMonoDamageable : MonoBehaviour, IDamageable {
        [SerializeField] private HurtBox[] _hurtBoxes;

        private void OnEnable() {
            foreach (HurtBox hurtBox in _hurtBoxes)
                hurtBox.Damageable = this;
        }

        public void TakeDamage(float damage) =>
            Debug.LogError("Taken damage " + damage);
    }
}