using _Project.Scripts.Core.Gameplay;
using _Project.Scripts.Core.Windows;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.StateMachines.GameplayStates {
    public class ChoosePowerUpState : IState {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly IWindowManager _windowManager;

        private PowerUpSelectionPresenter _powerUpSelectionPresenter;

        public ChoosePowerUpState(IWindowManager windowManager, GameplayStateMachine gameplayStateMachine) {
            _windowManager = windowManager;
            _gameplayStateMachine = gameplayStateMachine;
        }

        public void Enter() {
            Time.timeScale = 0;
            PowerUpSelectionWindow window = _windowManager.OpenWindow<PowerUpSelectionWindow>();
            _powerUpSelectionPresenter = window.GetComponent<PowerUpSelectionPresenter>();
            _powerUpSelectionPresenter.OnPowerUpApplied += OnPowerUpApplied;
            _powerUpSelectionPresenter.Show();
        }

        public void Exit() {
            Time.timeScale = 1;
            _powerUpSelectionPresenter.OnPowerUpApplied -= OnPowerUpApplied;
        }

        private void OnPowerUpApplied() {
            _windowManager.CloseWindow<PowerUpSelectionWindow>();
            _gameplayStateMachine.Enter<StartGameState>();
        }
    }
}