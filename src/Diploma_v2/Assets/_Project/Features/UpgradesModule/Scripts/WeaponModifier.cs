using System;
using _Project.Features.StatsModule;
using UnityEngine;

namespace _Project.Features.UpgradesModule {
    [Serializable]
    public class WeaponModifier
    {
        public WeaponStats statToModify;
        public float baseValue;
        public float valuePerLevel;
        public bool isPercentage;
    }
}