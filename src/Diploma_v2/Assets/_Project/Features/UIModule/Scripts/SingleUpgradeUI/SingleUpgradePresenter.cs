using System;
using _Project.Features.GameTimeModule;
using _Project.Features.UIModule.Windows;
using _Project.Features.UpgradesModule.API;
using _Project.Infrastructure.MVP.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Features.UIModule.SingleUpgradeUI {
    [PublicAPI]
    public class SingleUpgradePresenter : PresenterBehaviour<SingleUpgradeViewBase> {
        private readonly IWindowService _windowService;
        private readonly IUpgradeManagerService _upgradeManagerService;
        private readonly IGamePauseService _gamePauseService;
        private UpgradeData _upgradeData;

        public SingleUpgradePresenter(IWindowService windowService, IUpgradeManagerService upgradeManagerService, IGamePauseService gamePauseService) {
            _windowService = windowService;
            _upgradeManagerService = upgradeManagerService;
            _gamePauseService = gamePauseService;
        }

        public override void OnViewSet() =>
            View.OnClaimUpgradeClicked += ClaimUpgrade;

        public override void OnDisposed() =>
            View.OnClaimUpgradeClicked -= ClaimUpgrade;

        private void ClaimUpgrade() {
            _upgradeManagerService.TryApplyUpgrade(_upgradeData);
            _windowService.CloseWindow<ChooseUpgradeWindow>();
            _gamePauseService.ResumeTime();
        }

        public void SetUpgradeData(UpgradeData upgradeData) {
            _upgradeData = upgradeData;

            View.SetIconImage(upgradeData.icon);
            View.SetTitle(upgradeData.displayName);
            View.SetDescription(_upgradeManagerService.GetUpgradeDescription(upgradeData));
            View.SetPanelColor(GetUpgradeColor(upgradeData));
        }

        private Color GetUpgradeColor(UpgradeData upgradeData) {
            switch (upgradeData.rarity) {
                case UpgradeRarity.Common: return Color.white;
                case UpgradeRarity.Rare: return Color.blue;
                case UpgradeRarity.Epic: return Color.magenta;
                case UpgradeRarity.Legendary: return Color.yellow;
                default: throw new ArgumentOutOfRangeException(nameof(upgradeData.rarity), upgradeData.rarity, null);
            }
        }
    }
}