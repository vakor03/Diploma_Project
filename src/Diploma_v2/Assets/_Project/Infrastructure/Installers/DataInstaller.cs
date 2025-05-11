using _Project.Features.ExperienceModule;
using _Project.Features.PlayerModule;
using _Project.Features.PlayerSpawnerModule;
using _Project.Features.UIModule.ChooseUpgradesUI;
using Zenject;

namespace _Project.Scripts.Infrastructure.Installers {
    public class DataInstaller : Installer<DataInstaller> {
        public override void InstallBindings() {
            InstallModel<PlayerSpawnPointsModel>();
            InstallModel<EnemySpawnPointsModel>();
            InstallModel<ExperienceModel>();
            InstallModel<PlayerHealthModel>();
            InstallModel<UpgradesToShowModel>();
        }
        
        private void InstallModel<T> () where T : IModel =>
            Container.Bind<T>().AsSingle();
    }
}