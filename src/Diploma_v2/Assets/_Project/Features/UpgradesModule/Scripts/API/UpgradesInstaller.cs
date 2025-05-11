using _Project.Extensions.ZenjectExtensions;
using _Project.Features.UpgradesModule.UpgradePools;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Features.UpgradesModule.API {
    public class UpgradesInstaller : Installer<UpgradesInstaller> {
        public override void InstallBindings() {
            Container.BindConfigurationFromAddressables<UpgradeDatabase>(AssetPath.Configuration.UPGRADE_DATABASE).AsSingle();
            Container.Bind<IUpgradeApplicationService>().To<UpgradeApplicationService>().AsSingle();
            Container.Bind<IUpgradePoolService>().To<UpgradePoolService>().AsSingle();
            Container.Bind<IUpgradeValidationService>().To<UpgradeValidationService>().AsSingle();
            Container.Bind<IUpgradeManagerService>().To<UpgradeManagerService>().AsSingle();
        }
    }
}