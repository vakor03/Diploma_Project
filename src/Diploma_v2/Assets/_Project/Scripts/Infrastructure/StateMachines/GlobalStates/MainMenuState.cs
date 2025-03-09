using _Project.Scripts.Infrastructure.AssetProviders;
using _Project.Scripts.Infrastructure.SceneLoader;

namespace _Project.Scripts.Infrastructure.StateMachines.GlobalStates {
    public class MainMenuState : IState {
        private readonly ISceneLoader _sceneLoader;

        public MainMenuState(ISceneLoader sceneLoader) =>
            _sceneLoader = sceneLoader;

        public void Enter() =>
            _sceneLoader.Load(AssetPath.MAIN_MENU_SCENE_NAME);

        public void Exit() { }
    }
}