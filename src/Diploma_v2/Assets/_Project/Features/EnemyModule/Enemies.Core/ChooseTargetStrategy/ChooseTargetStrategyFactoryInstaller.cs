using Zenject;

namespace _Project.Features.EnemyModule.Enemies.Core.ChooseTargetStrategy {
    public class ChooseTargetStrategyFactoryInstaller : Installer<ChooseTargetStrategyFactoryInstaller> {
        public override void InstallBindings() =>
            Container.Bind<IChooseTargetStrategyFactory>()
                .To<ChooseTargetStrategyFactory>()
                .AsSingle();
    }
}