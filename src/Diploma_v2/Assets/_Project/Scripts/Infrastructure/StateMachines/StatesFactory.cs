using Zenject;

namespace _Project.Scripts.Infrastructure.StateMachines
{
    public class StatesFactory
    {
        private DiContainer _container;

        public StatesFactory(DiContainer container)
        {
            _container = container;
        }

        public TState Create<TState>() where TState : IState
        {
            return _container.Instantiate<TState>();
        }
    }
}