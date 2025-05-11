using System;
using UnityEngine;

namespace _Project.Features.UpgradesModule.API {
    public interface IUpgradeManagerService
    {
        public int GetUpgradeLevel(string upgradeId);
        public bool TryApplyUpgrade(UpgradeData upgrade);
        public bool CanApplyUpgrade(UpgradeData upgrade);
        public string GetUpgradeDescription(UpgradeData upgrade, int level = -1);
    
        public event Action<UpgradeData, int> OnUpgradeApplied;
    }
}