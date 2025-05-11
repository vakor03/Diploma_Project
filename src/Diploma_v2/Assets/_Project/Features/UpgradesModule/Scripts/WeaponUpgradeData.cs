using System;

namespace _Project.Features.UpgradesModule {
    [Serializable]
    public class WeaponUpgradeData
    {
        public string weaponId;
        public WeaponModifier[] modifiers;
    }
}