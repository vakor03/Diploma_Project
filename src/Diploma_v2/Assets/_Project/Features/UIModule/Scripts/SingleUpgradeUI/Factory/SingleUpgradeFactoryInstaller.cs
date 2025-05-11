using _Project.Extensions.ZenjectExtensions;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Features.UIModule.SingleUpgradeUI.Factory {
    public class SingleUpgradeFactoryInstaller : Installer<SingleUpgradeFactoryInstaller> {
        public override void InstallBindings() {
            Container.BindConfigurationFromAddressables<SingleUpgradeFactoryConfiguration>(AssetPath.Configuration
                .SINGLE_UPGRADE_FACTORY_CONFIGURATION).AsSingle();
            Container.Bind<ISingleUpgradeUIFactory>().To<SingleUpgradeUIFactory>().AsSingle();
        }
    }
}