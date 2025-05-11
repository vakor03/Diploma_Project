using System;

namespace _Project.Features.UpgradesModule {
    [Serializable]
    public enum StatType
    {
        Health = 0,
        AttackDamage = 1,
        AttackSpeed = 2,
        MovementSpeed = 3,
        Defense = 4,
        CritChance = 5,
        CritDamage = 6
    }
}