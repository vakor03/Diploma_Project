using System;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public interface IHealable {
        public event Action<float> OnHeal;

        public void Heal(float healAmount);
    }
}