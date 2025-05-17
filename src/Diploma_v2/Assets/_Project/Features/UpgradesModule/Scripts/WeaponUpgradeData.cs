using System;
using _Project.Features.UpgradesModule.API;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;

namespace _Project.Features.UpgradesModule {
    [Serializable]
    public class WeaponUpgradeData
    {
        public WeaponType weaponType;
        public WeaponModifier[] modifiers;
    }
}