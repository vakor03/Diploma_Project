using UnityEngine;
using Zenject;

namespace _Project.Features.UpgradesModule.API {
    public class UpgradeApplicationService : IUpgradeApplicationService
    {
        private readonly IStatService _statService;
        private readonly IPlayerWeaponService _playerWeaponService;
    
        // [Inject]
        // public UpgradeApplicationService(IStatService statService, IPlayerWeaponService playerWeaponService)
        // {
        //     _statService = statService;
        //     _playerWeaponService = playerWeaponService;
        // }
    
        public void ApplyUpgrade(UpgradeData upgrade, int currentLevel, GameObject target)
        {
            switch (upgrade.upgradeType)
            {
                case UpgradeType.Stat:
                    ApplyStatUpgrade(upgrade.statData, currentLevel + 1, target);
                    break;
                
                case UpgradeType.Weapon:
                    ApplyWeaponUpgrade(upgrade.weaponData, currentLevel + 1, target);
                    break;
                
                case UpgradeType.WeaponUnlock:
                    ApplyWeaponUnlock(upgrade.unlockData, target);
                    break;
            }
        }
    
        private void ApplyStatUpgrade(StatUpgradeData statData, int level, GameObject target)
        {
            float value = statData.baseValue + (statData.valuePerLevel * (level - 1));
            _statService.ModifyStat(target, statData.statType, value, statData.isPercentage);
        }
    
        private void ApplyWeaponUpgrade(WeaponUpgradeData weaponData, int level, GameObject target)
        {
            foreach (var modifier in weaponData.modifiers)
            {
                float value = modifier.baseValue + (modifier.valuePerLevel * (level - 1));
                _playerWeaponService.ModifyWeapon(target, weaponData.weaponId, modifier.modifierName, value, modifier.isPercentage);
            }
        }
    
        private void ApplyWeaponUnlock(WeaponUnlockData unlockData, GameObject target)
        {
            _playerWeaponService.UnlockWeapon(target, unlockData.weaponId, unlockData.weaponPrefab, unlockData.spawnOffset, unlockData.isPassiveWeapon);
        }
    
        public string GetUpgradeDescription(UpgradeData upgrade, int level)
        {
            switch (upgrade.upgradeType)
            {
                case UpgradeType.Stat:
                    return GetStatUpgradeDescription(upgrade.statData, level);
                
                case UpgradeType.Weapon:
                    return GetWeaponUpgradeDescription(upgrade.weaponData, level);
                
                case UpgradeType.WeaponUnlock:
                    return GetWeaponUnlockDescription(upgrade.unlockData, level);
                
                default:
                    return upgrade.description;
            }
        }
    
        private string GetStatUpgradeDescription(StatUpgradeData statData, int level)
        {
            float value = statData.baseValue + (statData.valuePerLevel * (level - 1));
            string suffix = statData.isPercentage ? "%" : "";
            return $"{statData.statType}: +{value}{suffix}";
        }
    
        private string GetWeaponUpgradeDescription(WeaponUpgradeData weaponData, int level)
        {
            var description = "";
            foreach (var modifier in weaponData.modifiers)
            {
                float value = modifier.baseValue + (modifier.valuePerLevel * (level - 1));
                string suffix = modifier.isPercentage ? "%" : "";
                description += $"{modifier.modifierName}: +{value}{suffix}\n";
            }
            return description.TrimEnd('\n');
        }
    
        private string GetWeaponUnlockDescription(WeaponUnlockData unlockData, int level)
        {
            if (level >= 1)
                return "Already Unlocked";
        
            return $"Unlock: {unlockData.weaponId}";
        }
    }
}