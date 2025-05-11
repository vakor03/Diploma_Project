using _Project.Features.CameraModule;
using _Project.Features.Enemy.EnemySpawner;
using _Project.Features.ExperienceModule;
using _Project.Features.Installers;
using _Project.Features.LevelGeneratorModule.TilemapsBootstrap;
using _Project.Features.MapGeneration;
using _Project.Features.MapGeneration.Matrix;
using _Project.Features.StatsModule;
using _Project.Features.UIModule;
using _Project.Features.UpgradesModule;
using _Project.Features.UpgradesModule.API;
using _Project.Infrastructure.MVP.Core;
using _Project.Scripts.Infrastructure.StateMachines;
using Zenject;

namespace _Project.Infrastructure.SceneContexts.Scripts
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InputSystemInstaller.Install(Container);
            SeedServiceInstaller.Install(Container);
            LevelGeneratorInstaller.Install(Container);
            PlayerSpawnerInstaller.Install(Container);
            MatrixInstaller.Install(Container);
            DungeonGeneratorInstaller.Install(Container);
            TilemapsInstaller.Install(Container);
            CameraInstaller.Install(Container);
            WindowServiceInstaller.Install(Container);
            EnemySpawnerInstaller.Install(Container);
            XPOrbsInstaller.Install(Container);
            ExperienceInstaller.Install(Container);
            UpgradesInstaller.Install(Container);
            GameplayUIInstaller.Install(Container);
            PlayerWeaponsInstaller.Install(Container);
            StatsInstaller.Install(Container);

            BindStatesFactory();

            BindGameplayStateMachine();
        }

        private void BindGameplayStateMachine() =>
            Container.Bind<GameplayStateMachine>().AsSingle();

        private void BindStatesFactory() =>
            Container.Bind<StatesFactory>().AsSingle();
    }
}