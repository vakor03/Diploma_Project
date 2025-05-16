namespace _Project.Features.EnemiesModule.Scripts.Enemies.Core {
    public interface IChooseTargetStrategyFactory {
        public IChooseTargetStrategy Create<TDerived>() where TDerived : IChooseTargetStrategy;
    }
}