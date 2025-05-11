using Zenject;

namespace _Project.Features.UpgradesModule.API {
    public class UpgradeValidationService : IUpgradeValidationService
    {
        // private readonly IPlayerWeaponService _playerWeaponService;
        //
        // [Inject]
        // public UpgradeValidationService(IPlayerWeaponService playerWeaponService) =>
        //     _playerWeaponService = playerWeaponService;

        public bool CanApplyUpgrade(UpgradeData upgrade, int currentLevel)
        {
            // Here you can add your custom logic for when upgrades can be applied
            // For example, based on player level, resources, etc.
        
            // switch (upgrade.upgradeType)
            // {
            //     case UpgradeType.WeaponUnlock:
            //         // Check if weapon is already unlocked
            //         return !_playerWeaponService.IsWeaponUnlocked(upgrade.unlockData.weaponId);
            //     
            //     case UpgradeType.Weapon:
            //         // Check if target weapon is unlocked
            //         return _playerWeaponService.IsWeaponUnlocked(upgrade.weaponData.weaponId);
            //     
            //     case UpgradeType.Stat:
            //         // Stats can always be upgraded - add your custom logic here
            //         return true;
            // }
            //
            // return false;

            return true;
        }
    }
}