namespace _Project.Scripts.Infrastructure.StateMachines
{
    public interface IStatesFactory
    {
        public TState Create<TState>() where TState : IState;
    }
}