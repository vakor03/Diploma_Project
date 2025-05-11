using UnityEngine;

namespace _Project.Features.UpgradesModule.API {
    public interface IUpgradeApplicationService
    {
        public void ApplyUpgrade(UpgradeData upgrade, int currentLevel);
        public string GetUpgradeDescription(UpgradeData upgrade, int level);
    }
}