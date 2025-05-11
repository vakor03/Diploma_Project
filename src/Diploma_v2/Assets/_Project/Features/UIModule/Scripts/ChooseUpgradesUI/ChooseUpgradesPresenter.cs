using System.Collections.Generic;
using _Project.Features.UIModule.SingleUpgradeUI.Factory;
using _Project.Features.UpgradesModule.API;
using _Project.Infrastructure.MVP.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Features.UIModule.ChooseUpgradesUI {
    [PublicAPI]
    public class ChooseUpgradesPresenter : PresenterBehaviour<ChooseUpgradesViewBase> {
        private readonly ISingleUpgradeUIFactory _singleUpgradeUIFactory;
        private readonly UpgradesToShowModel _upgradesToShowModel;
        private readonly UpgradeDatabase _upgradeDatabase;

        public ChooseUpgradesPresenter(ISingleUpgradeUIFactory singleUpgradeUIFactory, UpgradesToShowModel upgradesToShowModel, UpgradeDatabase upgradeDatabase) {
            _singleUpgradeUIFactory = singleUpgradeUIFactory;
            _upgradesToShowModel = upgradesToShowModel;
            _upgradeDatabase = upgradeDatabase;
        }

        public override void OnViewSet() {
            ClearChildren();
            CreateUpgrades();
        }

        private void ClearChildren() {
            foreach (Transform child in View.ContentParent)
                Object.Destroy(child.gameObject);
        }

        private void CreateUpgrades() {
            List<UpgradeData> upgrades = GetUpgrades();
            foreach (UpgradeData upgradeData in upgrades)
                CreateUpgrade(upgradeData);
        }

        private List<UpgradeData> GetUpgrades() =>
            _upgradesToShowModel.UpgradesToShow.ConvertAll(el=> _upgradeDatabase.GetUpgradeById(el));

        private void CreateUpgrade(UpgradeData upgradeData) =>
            _singleUpgradeUIFactory.CreateSingleUpgradeForUpgrade(upgradeData, View.ContentParent);
    }
}