using _Project.Scripts.Infrastructure.StateMachines;
using _Project.Scripts.Infrastructure.StateMachines.GlobalStates;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Bootstraps
{
    public class Bootstrapper : MonoBehaviour
    {
        private GlobalStateMachine _globalStateMachine;
        private StatesFactory _statesFactory;

        [Inject]
        private void Construct(GlobalStateMachine globalStateMachine, StatesFactory statesFactory)
        {
            _globalStateMachine = globalStateMachine;
            _statesFactory = statesFactory;
        }

        private void Start()
        {
            _globalStateMachine.RegisterState(_statesFactory.Create<MainMenuState>());
            _globalStateMachine.RegisterState(_statesFactory.Create<HubState>());
            _globalStateMachine.RegisterState(_statesFactory.Create<GameplayState>());
            _globalStateMachine.RegisterState(_statesFactory.Create<QuitState>());
            
            _globalStateMachine.Enter<MainMenuState>();
            DontDestroyOnLoad(this);
        }
    }
}