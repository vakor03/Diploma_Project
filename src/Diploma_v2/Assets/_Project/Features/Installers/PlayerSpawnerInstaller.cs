using _Project.Features.PlayerSpawnerModule;
using Zenject;

namespace _Project.Features.Installers
{
    public class PlayerSpawnerInstaller : Installer<PlayerSpawnerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PlayerSpawnerService>().AsSingle();
            Container.Bind<PlayerSpawnPointMarker>().FromComponentsInHierarchy().AsSingle();
            Container.Bind<IPlayerSpawnPointsService>().To<PlayerSpawnPointsService>().AsSingle();
        }
    }
}