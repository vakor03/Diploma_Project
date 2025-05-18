using _Project.Features.ExperienceModule;
using _Project.Features.GameTimeModule;
using _Project.Features.LevelGeneratorModule;
using _Project.Features.PlayerModule;
using _Project.Features.PlayerSpawnerModule;
using _Project.Features.UIModule.ChooseUpgradesUI;
using _Project.Features.UIModule.SingleUpgradeUI;
using _Project.Features.UpgradesModule.API;
using _Project.Scripts.Infrastructure;
using Zenject;

namespace _Project.Infrastructure.Installers {
    public class DataInstaller : Installer<DataInstaller> {
        public override void InstallBindings() {
            InstallModel<PlayerSpawnPointsModel>();
            InstallModel<EnemySpawnPointsModel>();
            InstallModel<ExperienceModel>();
            InstallModel<UpgradesToShowModel>();
            InstallModel<GamePauseModel>();
            InstallModel<PlayerWeaponModel>();
            InstallModel<PlayerStatsModel>();
            InstallModel<BlockGroupsModel>();

            Container.Bind<UpgradeEvents>().AsSingle();
        }
        
        private void InstallModel<T> () where T : IModel =>
            Container.Bind<T>().AsSingle();
    }
}