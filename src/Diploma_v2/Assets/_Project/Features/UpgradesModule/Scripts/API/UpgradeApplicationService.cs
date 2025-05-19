using _Project.Features.StatsModule;
using _Project.Features.PlayerSpawnerModule;

namespace _Project.Features.UpgradesModule.API {
    public class UpgradeApplicationService : IUpgradeApplicationService {
        private readonly IPlayerWeaponService _playerWeaponService;
        private readonly PlayerStatsModel _playerStatsModel;

        public UpgradeApplicationService(IPlayerWeaponService playerWeaponService, PlayerStatsModel playerStatsModel) {
            _playerWeaponService = playerWeaponService;
            _playerStatsModel = playerStatsModel;
        }

        public void ApplyUpgrade(UpgradeData upgrade, int currentLevel) {
            switch (upgrade.upgradeType) {
                case UpgradeType.Stat: ApplyStatUpgrade(upgrade.statData, currentLevel + 1); break;
                case UpgradeType.Weapon: ApplyWeaponUpgrade(upgrade.weaponData, currentLevel + 1); break;
                case UpgradeType.WeaponUnlock: ApplyWeaponUnlock(upgrade.unlockData); break;
            }
        }

        private void ApplyStatUpgrade(StatUpgradeData statData, int level) {
            float value = statData.baseValue + (statData.valuePerLevel * (level - 1));
            if (statData.isPercentage)
                _playerStatsModel.Stats.ModifyStatPercentage(statData.statType, value);
            else
                _playerStatsModel.Stats.ModifyStat(statData.statType, value);
        }

        private void ApplyWeaponUpgrade(WeaponUpgradeData weaponData, int level) {
            foreach (WeaponModifier modifier in weaponData.modifiers) {
                float value = modifier.baseValue + (modifier.valuePerLevel * (level - 1));
                _playerWeaponService.ModifyWeapon(weaponData.weaponType, modifier.statToModify, value, modifier.isPercentage);
            }
        }

        private void ApplyWeaponUnlock(WeaponUnlockData unlockData) =>
            _playerWeaponService.UnlockWeapon(unlockData.weaponType);

        public string GetUpgradeDescription(UpgradeData upgrade, int level) {
            switch (upgrade.upgradeType) {
                case UpgradeType.Stat: return GetStatUpgradeDescription(upgrade.statData, level);
                case UpgradeType.Weapon: return GetWeaponUpgradeDescription(upgrade.weaponData, level);
                case UpgradeType.WeaponUnlock: return GetWeaponUnlockDescription(upgrade.unlockData, level);
                default: return upgrade.description;
            }
        }

        private string GetStatUpgradeDescription(StatUpgradeData statData, int level) {
            float value = statData.baseValue + (statData.valuePerLevel * (level - 1));
            string suffix = statData.isPercentage ? "%" : "";
            return $"{statData.statType}: +{value}{suffix}";
        }

        private string GetWeaponUpgradeDescription(WeaponUpgradeData weaponData, int level) {
            var description = "";
            foreach (var modifier in weaponData.modifiers) {
                float value = modifier.baseValue + (modifier.valuePerLevel * (level - 1));
                string suffix = modifier.isPercentage ? "%" : "";
                description += $"{modifier.statToModify.ToString()}: +{value}{suffix}\n";
            }
            return description.TrimEnd('\n');
        }

        private string GetWeaponUnlockDescription(WeaponUnlockData unlockData, int level) {
            if (level >= 1)
                return "Already Unlocked";
            return $"Unlock: {unlockData.weaponType.ToString()}";
        }
    }
}