using Zenject;

namespace _Project.Scripts.Infrastructure.StateMachines
{
    public class StatesFactory : IStatesFactory
    {
        private readonly IInstantiator _instantiator;

        public StatesFactory(IInstantiator instantiator) =>
            _instantiator = instantiator;

        public TState Create<TState>() where TState : IState =>
            _instantiator.Instantiate<TState>();
    }
}