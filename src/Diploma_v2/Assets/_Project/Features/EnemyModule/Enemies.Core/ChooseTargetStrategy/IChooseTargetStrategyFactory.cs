namespace _Project.Features.EnemyModule.Enemies.Core.ChooseTargetStrategy {
    public interface IChooseTargetStrategyFactory {
        public IChooseTargetStrategy Create<TDerived>() where TDerived : IChooseTargetStrategy;
    }
}