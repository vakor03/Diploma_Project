using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Features.UpgradesModule.API {
    public class UpgradeManagerService : IUpgradeManagerService
    {
        private readonly Dictionary<string, int> _upgradeLevels = new Dictionary<string, int>();
        private readonly IUpgradeValidationService _validationService;
        private readonly IUpgradeApplicationService _applicationService;
    
        public event Action<UpgradeData, int> OnUpgradeApplied;
    
        [Inject]
        public UpgradeManagerService(
            IUpgradeValidationService validationService,
            IUpgradeApplicationService applicationService)
        {
            _validationService = validationService;
            _applicationService = applicationService;
        }
    
        public int GetUpgradeLevel(string upgradeId)
        {
            _upgradeLevels.TryGetValue(upgradeId, out int level);
            return level;
        }
    
        public bool TryApplyUpgrade(UpgradeData upgrade, GameObject target)
        {
            int currentLevel = GetUpgradeLevel(upgrade.upgradeId);
        
            if (!_validationService.CanApplyUpgrade(upgrade, currentLevel))
            {
                return false;
            }
        
            // Apply upgrade
            _applicationService.ApplyUpgrade(upgrade, currentLevel, target);
        
            // Update level
            _upgradeLevels[upgrade.upgradeId] = currentLevel + 1;
        
            // Trigger event
            OnUpgradeApplied?.Invoke(upgrade, currentLevel + 1);
        
            return true;
        }
    
        public bool CanApplyUpgrade(UpgradeData upgrade)
        {
            int currentLevel = GetUpgradeLevel(upgrade.upgradeId);
            return _validationService.CanApplyUpgrade(upgrade, currentLevel);
        }
    
        public string GetUpgradeDescription(UpgradeData upgrade, int level = -1)
        {
            if (level < 0)
                level = GetUpgradeLevel(upgrade.upgradeId) + 1;
            
            return _applicationService.GetUpgradeDescription(upgrade, level);
        }
    }
}