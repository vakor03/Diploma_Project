using _Project.Features.ExperienceModule;
using _Project.Features.PlayerSpawnerModule;
using Zenject;

namespace _Project.Scripts.Infrastructure.Installers {
    public class DataInstaller : Installer<DataInstaller> {
        public override void InstallBindings() {
            InstallModel<PlayerSpawnPointsModel>();
            InstallModel<EnemySpawnPointsModel>();
            InstallModel<ExperienceModel>();
        }
        
        private void InstallModel<T> () where T : IModel =>
            Container.Bind<T>().AsSingle();
    }
}