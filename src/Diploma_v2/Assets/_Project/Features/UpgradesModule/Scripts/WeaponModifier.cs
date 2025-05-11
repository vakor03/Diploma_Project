using System;
using UnityEngine;

namespace _Project.Features.UpgradesModule {
    [Serializable]
    public class WeaponModifier
    {
        public string modifierName;
        public float baseValue;
        public float valuePerLevel;
        public bool isPercentage;
        [TextArea] public string modifierDescription;
    }
}