using _Project.Features.UIModule.Windows;
using _Project.Features.UpgradesModule.API;
using _Project.Infrastructure.MVP.Core;
using UnityEngine;

namespace _Project.Features.UIModule.SingleUpgradeUI.Factory {
    public class SingleUpgradeUIFactory : ISingleUpgradeUIFactory {
        private readonly IWindowService _windowService;
        private readonly SingleUpgradeFactoryConfiguration _configuration;
        
        public SingleUpgradeUIFactory(IWindowService windowService, SingleUpgradeFactoryConfiguration configuration) {
            _windowService = windowService;
            _configuration = configuration;
        }

        public SingleUpgradePresenter CreateSingleUpgradeForUpgrade(UpgradeData upgradeData, RectTransform parent) {
            SingleUpgradePresenter presenter = _windowService.CreatePresenterForWindow<ChooseUpgradeWindow, SingleUpgradePresenter>(parent,
                _configuration.SingleUpgradePrefab);
            
            presenter.SetUpgradeData(upgradeData);
            return presenter;
        }
    }
}