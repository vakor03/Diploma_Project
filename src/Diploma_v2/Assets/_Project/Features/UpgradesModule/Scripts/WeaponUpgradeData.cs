using System;
using _Project.Features.UpgradesModule.API;
using Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;

namespace _Project.Features.UpgradesModule {
    [Serializable]
    public class WeaponUpgradeData
    {
        public WeaponType weaponType;
        public WeaponModifier[] modifiers;
    }
}