using Zenject;

namespace _Project.Features.EnemiesModule.Scripts.Enemies.Core {
    public class ChooseTargetStrategyFactory : IChooseTargetStrategyFactory {
        private readonly DiContainer _diContainer;

        public ChooseTargetStrategyFactory(DiContainer diContainer) =>
            _diContainer = diContainer;

        public IChooseTargetStrategy Create<TDerived>() where TDerived : IChooseTargetStrategy =>
            _diContainer.Instantiate<TDerived>();
    }
}