using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Features.UpgradesModule.API {
    [CreateAssetMenu(fileName = nameof(UpgradeDatabase) + "_Default", menuName = "Configurations/UpgradesModule/" + nameof(UpgradeDatabase))]
    public class UpgradeDatabase : ScriptableObject
    {
        [Header("All Upgrades")]
        [SerializeField] private List<UpgradeData> allUpgrades;
    
        public List<UpgradeData> AllUpgrades => allUpgrades;
    
        public UpgradeData GetUpgradeById(string upgradeId) =>
            allUpgrades.FirstOrDefault(u => u.upgradeId == upgradeId);

        public List<UpgradeData> GetUpgradesByRarity(UpgradeRarity rarity) =>
            allUpgrades.Where(u => u.rarity == rarity).ToList();

        public List<UpgradeData> GetUpgradesByType(UpgradeType type) =>
            allUpgrades.Where(u => u.upgradeType == type).ToList();
    }
}