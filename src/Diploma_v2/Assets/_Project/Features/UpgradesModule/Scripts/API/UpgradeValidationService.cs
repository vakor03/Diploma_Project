namespace _Project.Features.UpgradesModule.API {
    public class UpgradeValidationService : IUpgradeValidationService {
        private readonly IPlayerWeaponService _playerWeaponService;
        
        public UpgradeValidationService(IPlayerWeaponService playerWeaponService) =>
            _playerWeaponService = playerWeaponService;

        public bool CanApplyUpgrade(UpgradeData upgrade, int currentLevel) {
            switch (upgrade.upgradeType)
            {
                case UpgradeType.WeaponUnlock:
                    return !_playerWeaponService.IsWeaponUnlocked(upgrade.unlockData.weaponType);
                
                case UpgradeType.Weapon:
                    return _playerWeaponService.IsWeaponUnlocked(upgrade.weaponData.weaponType);
                
                case UpgradeType.Stat:
                    return true;
            }
            
            return false;
        }
    }
}