namespace _Project.Scripts.Infrastructure.StateMachines
{
    public interface IStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void RegisterState<TState>(TState state) where TState : IState;
    }
}