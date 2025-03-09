using _Project.Scripts.Infrastructure.AssetProviders;
using _Project.Scripts.Infrastructure.SceneLoader;

namespace _Project.Scripts.Infrastructure.StateMachines.GlobalStates {
    public class GameplayState : IState {
        private readonly ISceneLoader _sceneLoader;

        public GameplayState(ISceneLoader sceneLoader) =>
            _sceneLoader = sceneLoader;

        public void Enter() =>
            _sceneLoader.Load(AssetPath.GAME_SCENE_NAME);

        public void Exit() { }
    }
}