using _Project.Extensions.ZenjectExtensions;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Features.LevelGeneratorModule
{
    public class LevelGeneratorInstaller : Installer<LevelGeneratorInstaller>
    {
        public override void InstallBindings() {
            Container.BindConfigurationFromAddressables<TilemapsCollidersConfiguration>(AssetPath.Configuration.TILEMAPS_COLLIDERS_CONFIGURATION)
                .AsSingle();
            Container.Bind<IBlockGroupService>().To<BlockGroupService>().AsSingle();
            Container.Bind<ILevelGenerationService>().To<LevelGenerationService>().AsSingle();
        }
    }
}