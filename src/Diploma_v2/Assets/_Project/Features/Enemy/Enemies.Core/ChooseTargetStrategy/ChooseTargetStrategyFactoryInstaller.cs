using Zenject;

namespace _Project.Features.EnemiesModule.Scripts.Enemies.Core {
    public class ChooseTargetStrategyFactoryInstaller : Installer<ChooseTargetStrategyFactoryInstaller> {
        public override void InstallBindings() =>
            Container.Bind<IChooseTargetStrategyFactory>()
                .To<ChooseTargetStrategyFactory>()
                .AsSingle();
    }
}