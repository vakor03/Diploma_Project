using _Project.Features.UIModule.SingleUpgradeUI.Factory;
using Zenject;

namespace _Project.Features.UIModule {
    public class GameplayUIInstaller : Installer<GameplayUIInstaller> {
        public override void InstallBindings() {
            SingleUpgradeFactoryInstaller.Install(Container);
        }
    }
}