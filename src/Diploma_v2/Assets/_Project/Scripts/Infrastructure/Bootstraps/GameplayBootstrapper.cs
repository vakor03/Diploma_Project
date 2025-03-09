using _Project.Scripts.Infrastructure.StateMachines;
using _Project.Scripts.Infrastructure.StateMachines.GameplayStates;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Bootstraps
{
    public class GameplayBootstrapper : MonoBehaviour
    {
        private GameplayStateMachine _gameplayStateMachine;
        private StatesFactory _statesFactory;

        [Inject]
        private void Construct(GameplayStateMachine gameplayStateMachine, 
            StatesFactory statesFactory)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _statesFactory = statesFactory;
        }

        private void Start()
        {
            _gameplayStateMachine.RegisterState(_statesFactory.Create<InitGameState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<StartGameState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<GamePauseState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<GameOverState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<ChoosePowerUpState>());

            _gameplayStateMachine.Enter<InitGameState>();
        }
    }
}