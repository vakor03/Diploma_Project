using _Project.Features.UpgradesModule.API;
using UnityEngine;

namespace _Project.Features.UIModule.SingleUpgradeUI.Factory {
    public interface ISingleUpgradeUIFactory {
        public SingleUpgradePresenter CreateSingleUpgradeForUpgrade(UpgradeData upgradeData, RectTransform parent);
    }
}