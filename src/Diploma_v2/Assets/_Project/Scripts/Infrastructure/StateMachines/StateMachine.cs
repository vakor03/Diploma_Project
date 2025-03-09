using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.StateMachines
{
    public abstract class StateMachine : IStateMachine
    {
        private readonly Dictionary<System.Type, IState> _registeredStates = new();
        private IState _currentState;

        public void Enter<TState>() where TState : class, IState
        {
            var newState = ChangeState<TState>();
            newState.Enter();
        }

        public void RegisterState<TState>(TState state) where TState : IState
        {
            _registeredStates.Add(typeof(TState), state);
        }

        private TState ChangeState<TState>() where TState : class, IState
        {
            if (_currentState != null)
                _currentState.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IState
        {
            return _registeredStates[typeof(TState)] as TState;
        }
    }
}