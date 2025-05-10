using _Project.Features.PlayerSpawnerModule;
using Zenject;

namespace _Project.Features.Installers
{
    public class PlayerSpawnerInstaller : Installer<PlayerSpawnerInstaller>
    {
        public override void InstallBindings() {
            Container.Bind<PlayerTransformDataHolder>().AsSingle();
            Container.BindInterfacesTo<PlayerSpawnerService>().AsSingle();
            Container.Bind<IPlayerSpawnPointsService>().To<PlayerSpawnPointsService>().AsSingle();
        }
    }
}