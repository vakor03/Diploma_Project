using _Project.Scripts.Infrastructure.AssetProviders;
using _Project.Scripts.Infrastructure.SceneLoader;

namespace _Project.Scripts.Infrastructure.StateMachines.GlobalStates {
    public class HubState : IState {
        private readonly ISceneLoader _sceneLoader;

        public HubState(ISceneLoader sceneLoader) =>
            _sceneLoader = sceneLoader;

        public void Enter() {
            // _sceneLoader.Unload(AssetPath.MAIN_MENU_SCENE_NAME);
            // _sceneLoader.Load(AssetPath.HUB_SCENE_NAME);
        }

        public void Exit() { }
    }
}