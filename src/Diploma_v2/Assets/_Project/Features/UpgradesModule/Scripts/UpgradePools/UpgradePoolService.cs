using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Features.UpgradesModule.API;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Features.UpgradesModule.UpgradePools {
    public class UpgradePoolService : IUpgradePoolService {
        private readonly UpgradeDatabase _upgradeDatabase;
        private readonly IUpgradeManagerService _upgradeManager;
        
        public UpgradePoolService(UpgradeDatabase upgradeDatabase, IUpgradeManagerService upgradeManager) {
            _upgradeDatabase = upgradeDatabase;
            _upgradeManager = upgradeManager;
        }

        public List<UpgradeData> GetRandomUpgrades(int playerLevel, float luckStat, int count = 3) {
            List<UpgradeRarity> rarities = SelectRarities(playerLevel, luckStat, count);
            List<UpgradeData> selectedUpgrades = new List<UpgradeData>();
            HashSet<string> selectedUpgradeIds = new HashSet<string>();

            foreach (UpgradeRarity rarity in rarities) {
                UpgradeData selectedUpgrade = SelectUpgradeWithFallback(rarity, selectedUpgradeIds);

                if (selectedUpgrade != null) {
                    selectedUpgrades.Add(selectedUpgrade);
                    selectedUpgradeIds.Add(selectedUpgrade.upgradeId);
                }
            }

            return selectedUpgrades;
        }

        private UpgradeData SelectUpgradeWithFallback(UpgradeRarity rarity, HashSet<string> excludeIds) {
            UpgradeData upgrade = TryGetRandomUpgrade(rarity, excludeIds);
            if (upgrade != null)
                return upgrade;

            switch (rarity) {
                case UpgradeRarity.Legendary: return SelectUpgradeWithFallback(UpgradeRarity.Epic, excludeIds);

                case UpgradeRarity.Epic: return SelectUpgradeWithFallback(UpgradeRarity.Rare, excludeIds);

                case UpgradeRarity.Rare: return SelectUpgradeWithFallback(UpgradeRarity.Common, excludeIds);

                case UpgradeRarity.Common:
                default: throw new ArgumentException("No valid upgrades found for the selected rarity.");
            }
        }

        private UpgradeData TryGetRandomUpgrade(UpgradeRarity rarity, HashSet<string> excludeIds) {
            List<UpgradeData> validUpgrades = GetValidUpgradesOfRarity(rarity);

            validUpgrades = validUpgrades.Where(u => !excludeIds.Contains(u.upgradeId)).ToList();

            if (validUpgrades.Count == 0)
                return null;

            return validUpgrades[Random.Range(0, validUpgrades.Count)];
        }

        public List<UpgradeRarity> SelectRarities(int playerLevel, float luckStat, int count) {
            List<UpgradeRarity> selectedRarities = new List<UpgradeRarity>();

            for (int i = 0; i < count; i++)
                selectedRarities.Add(CalculateRandomRarity(playerLevel, luckStat));

            return selectedRarities;
        }

        public List<UpgradeData> GetValidUpgradesOfRarity(UpgradeRarity rarity) {
            List<UpgradeData> upgradesOfRarity = _upgradeDatabase.GetUpgradesByRarity(rarity);

            List<UpgradeData> validUpgrades = new List<UpgradeData>();
            foreach (UpgradeData upgrade in upgradesOfRarity)
                if (_upgradeManager.CanApplyUpgrade(upgrade))
                    validUpgrades.Add(upgrade);

            return validUpgrades;
        }

        private UpgradeRarity CalculateRandomRarity(int playerLevel, float luckStat) {
            // Calculate base chances that increase with level and luck
            float luckBonus = luckStat * 0.01f; // 1% per luck point
            float levelBonus = (playerLevel - 1) * 0.02f; // 2% per level above 1

            // Base chances for each rarity
            float legendaryChance = 0.02f + luckBonus + levelBonus; // 2% base
            float epicChance = 0.15f + luckBonus + levelBonus; // 15% base
            float rareChance = 0.33f; // 33% base
            // Common is the remainder

            // Clamp values to prevent overflow
            legendaryChance = Mathf.Clamp(legendaryChance, 0f, 0.5f);
            epicChance = Mathf.Clamp(epicChance, 0f, 0.6f);

            // Roll for rarity
            float roll = Random.value;

            if (roll < legendaryChance)
                return UpgradeRarity.Legendary;
            else if (roll < legendaryChance + epicChance)
                return UpgradeRarity.Epic;
            else if (roll < legendaryChance + epicChance + rareChance)
                return UpgradeRarity.Rare;
            else
                return UpgradeRarity.Common;
        }
    }
}