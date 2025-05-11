using System.Collections.Generic;
using _Project.Scripts.Infrastructure;

namespace _Project.Features.UIModule.ChooseUpgradesUI {
    public class UpgradesToShowModel : IModel {
        public List<string> UpgradesToShow { get; private set; } = new();

        public void SetUpgradesToShow(List<string> upgradesToShow) =>
            UpgradesToShow = upgradesToShow;
    }
}