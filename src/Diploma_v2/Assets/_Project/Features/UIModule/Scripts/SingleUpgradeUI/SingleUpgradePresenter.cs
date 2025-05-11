using System;
using _Project.Features.UpgradesModule.API;
using _Project.Infrastructure.MVP.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Features.UIModule.SingleUpgradeUI {
    [PublicAPI]
    public class SingleUpgradePresenter : PresenterBehaviour<SingleUpgradeViewBase> {
        private UpgradeData _upgradeData;
        public override void OnViewSet() =>
            View.OnClaimUpgradeClicked += ClaimUpgrade;

        public override void OnDisposed() =>
            View.OnClaimUpgradeClicked -= ClaimUpgrade;

        private void ClaimUpgrade() =>
            Debug.LogError($"[SingleUpgradePresenter.ClaimUpgrade Line 15]");

        public void SetUpgradeData(UpgradeData upgradeData) {
            _upgradeData = upgradeData;
            
            View.SetIconImage(upgradeData.icon);
            View.SetTitle(upgradeData.displayName);
            View.SetDescription(upgradeData.description);
            View.SetPanelColor(GetUpgradeColor(upgradeData));
        }

        private Color GetUpgradeColor(UpgradeData upgradeData) {
            switch (upgradeData.rarity) {
                case UpgradeRarity.Common:
                    return Color.white;
                case UpgradeRarity.Rare:
                    return Color.blue;
                case UpgradeRarity.Epic:
                    return Color.magenta;
                case UpgradeRarity.Legendary:
                    return Color.yellow;
                default:
                    throw new ArgumentOutOfRangeException(nameof(upgradeData.rarity), upgradeData.rarity, null);
            }
        }
    }
}