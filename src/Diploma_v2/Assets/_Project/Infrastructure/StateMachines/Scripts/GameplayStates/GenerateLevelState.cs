using _Project.Extensions.UniTasks;
using _Project.Features.LevelGeneratorModule;
using _Project.Scripts.Infrastructure.StateMachines;
using Cysharp.Threading.Tasks;

namespace _Project.Infrastructure.StateMachines.Scripts.GameplayStates {
    public class GenerateLevelState : IState {
        private readonly ILevelGenerationService _levelGenerationService;
        private readonly GameplayStateMachine _stateMachine;

        public GenerateLevelState(ILevelGenerationService levelGenerationService, GameplayStateMachine stateMachine) {
            _levelGenerationService = levelGenerationService;
            _stateMachine = stateMachine;
        }

        public void Enter() {
            _levelGenerationService.Generate();

            UniTaskHelper.ExecuteAfterOneFrame(() => { _stateMachine.Enter<PrepareSceneState>(); }, PlayerLoopTiming.FixedUpdate)
                .Forget();
        }
    }
}