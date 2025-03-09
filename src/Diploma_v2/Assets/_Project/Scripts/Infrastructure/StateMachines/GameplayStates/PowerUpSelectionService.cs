namespace _Project.Scripts.Infrastructure.StateMachines.GameplayStates {
    public class PowerUpSelectionService : IPowerUpSelectionService {
        private readonly GameplayStateMachine _gameplayStateMachine;

        public PowerUpSelectionService(GameplayStateMachine gameplayStateMachine) =>
            _gameplayStateMachine = gameplayStateMachine;

        public void StartPowerUpSelection() =>
            _gameplayStateMachine.Enter<ChoosePowerUpState>();
    }
}