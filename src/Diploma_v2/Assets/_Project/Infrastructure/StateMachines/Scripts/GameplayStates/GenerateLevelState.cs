using _Project.Extensions.UniTasks;
using _Project.Features.GameTimeModule;
using _Project.Features.LevelGeneratorModule;
using _Project.Scripts.Infrastructure.StateMachines;
using Cysharp.Threading.Tasks;

namespace _Project.Infrastructure.StateMachines.Scripts.GameplayStates {
    public class GenerateLevelState : IState {
        private readonly ILevelGenerationService _levelGenerationService;
        private readonly GameplayStateMachine _stateMachine;
        private readonly IGamePauseService _gamePauseService;

        public GenerateLevelState(ILevelGenerationService levelGenerationService, GameplayStateMachine stateMachine, IGamePauseService gamePauseService) {
            _levelGenerationService = levelGenerationService;
            _stateMachine = stateMachine;
            _gamePauseService = gamePauseService;
        }

        public void Enter() {
            _levelGenerationService.Generate();
            _gamePauseService.ForceResumeTime();

            UniTaskHelper.ExecuteAfterOneFrame(() => { _stateMachine.Enter<PrepareSceneState>(); }, PlayerLoopTiming.FixedUpdate)
                .Forget();
        }
    }
}