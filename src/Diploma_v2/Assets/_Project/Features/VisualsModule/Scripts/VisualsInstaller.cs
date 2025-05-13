using _Project.Extensions.ZenjectExtensions;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Features.VisualsModule.Scripts {
    public class VisualsInstaller : Installer<VisualsInstaller> {
        public override void InstallBindings() {
            Container.BindConfigurationFromAddressables<VisualsConfiguration>(AssetPath.Configuration.VISUALS_CONFIGURATION)
                .AsSingle();
        }
    }
}