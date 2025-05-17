using _Project.Features.GameTimeModule;
using _Project.Features.UIModule.Windows;
using _Project.Infrastructure.MVP.Core;

namespace _Project.Scripts.Infrastructure.StateMachines.GameplayStates {
    public class GamePauseState : IState {
        private readonly IWindowService _windowService;
        private readonly IGamePauseService _gamePauseService;
        
        public GamePauseState(IWindowService windowService, IGamePauseService gamePauseService) {
            _windowService = windowService;
            _gamePauseService = gamePauseService;
        }

        public void Enter() {
            _gamePauseService.PauseTime();
            _windowService.ShowWindow<GamePauseWindow>();
        }

        public void Exit() {
            _gamePauseService.ResumeTime();
            _windowService.CloseWindow<GamePauseWindow>();
        }
    }
}