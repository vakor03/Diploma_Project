using _Project.Extensions.ZenjectExtensions;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
    public class GlobalConfigurationsInstaller : Installer<GlobalConfigurationsInstaller> {
        public override void InstallBindings() {
            Container.BindConfigurationFromAddressables<LayersConfiguration>(AssetPath.Configuration.LAYERS_CONFIGURATION)
                .AsSingle();
        }
    }
}