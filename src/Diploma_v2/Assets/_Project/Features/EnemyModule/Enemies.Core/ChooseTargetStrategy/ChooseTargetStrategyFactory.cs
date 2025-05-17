using Zenject;

namespace _Project.Features.EnemyModule.Enemies.Core.ChooseTargetStrategy {
    public class ChooseTargetStrategyFactory : IChooseTargetStrategyFactory {
        private readonly DiContainer _diContainer;

        public ChooseTargetStrategyFactory(DiContainer diContainer) =>
            _diContainer = diContainer;

        public IChooseTargetStrategy Create<TDerived>() where TDerived : IChooseTargetStrategy =>
            _diContainer.Instantiate<TDerived>();
    }
}