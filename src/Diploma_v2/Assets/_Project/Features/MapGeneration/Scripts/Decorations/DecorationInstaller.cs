using _Project.Extensions.ZenjectExtensions;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Features.MapGeneration.Decorations {
    public class DecorationInstaller : Installer<DecorationInstaller> {
        public override void InstallBindings() {
            Container.BindConfigurationFromAddressables<DecorationConfiguration>(AssetPath.Configuration.DECORATION_CONFIGURATION)
                .AsSingle();
            Container.Bind<IDecorationFactory>().To<DecorationFactory>().AsSingle();
        }
    }
}