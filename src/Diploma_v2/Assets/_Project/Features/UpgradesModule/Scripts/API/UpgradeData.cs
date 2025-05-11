using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Features.UpgradesModule.API {
    [CreateAssetMenu(fileName = nameof(UpgradeData) + "_Default", menuName = "Configurations/UpgradesModule/" + nameof(UpgradeData))]
    public class UpgradeData : ScriptableObject
    {
        [Header("Basic Info")]
        public string upgradeId;
        public string displayName;
        [TextArea] public string description;
        public UpgradeRarity rarity;
        public UpgradeType upgradeType;
        public Sprite icon;
        
        [ShowIf(nameof(upgradeType), UpgradeType.Stat)]
        public StatUpgradeData statData;
    
        [ShowIf(nameof(upgradeType), UpgradeType.Weapon)]
        public WeaponUpgradeData weaponData;
    
        [ShowIf(nameof(upgradeType), UpgradeType.WeaponUnlock)]
        public WeaponUnlockData unlockData;
    
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(upgradeId))
            {
                upgradeId = name.ToLower().Replace(" ", "_");
            }
        }
    }
}