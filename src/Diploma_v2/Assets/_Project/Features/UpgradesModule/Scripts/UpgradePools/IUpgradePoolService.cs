using System.Collections.Generic;
using _Project.Features.UpgradesModule.API;

namespace _Project.Features.UpgradesModule.UpgradePools {
    public interface IUpgradePoolService
    {
        public List<UpgradeData> GetRandomUpgrades(int playerLevel, float luckStat, int count = 3);
        public List<UpgradeRarity> SelectRarities(int playerLevel, float luckStat, int count);
        public List<UpgradeData> GetValidUpgradesOfRarity(UpgradeRarity rarity);
    }
}