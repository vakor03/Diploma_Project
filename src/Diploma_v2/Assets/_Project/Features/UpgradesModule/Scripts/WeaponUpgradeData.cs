using System;
using _Project.Features.UpgradesModule.API;

namespace _Project.Features.UpgradesModule {
    [Serializable]
    public class WeaponUpgradeData
    {
        public WeaponType weaponType;
        public WeaponModifier[] modifiers;
    }
}