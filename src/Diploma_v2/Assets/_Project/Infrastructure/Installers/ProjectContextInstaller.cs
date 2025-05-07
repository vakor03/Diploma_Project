using _Project.Features.MapGeneration;
using _Project.Features.MapGeneration.Matrix;
using _Project.Scripts.Infrastructure.AssetProviders;
using _Project.Scripts.Infrastructure.StateMachines;
using Zenject;

namespace _Project.Scripts.Infrastructure.Installers {
    public class ProjectContextInstaller : MonoInstaller<ProjectContextInstaller> {
        public override void InstallBindings() {
            DataInstaller.Install(Container);
            MatrixInstaller.Install(Container);

            BindSceneLoader();

            BindStaticDataService();

            BindAssetProvider();

            BindStatesFactory();

            BindGameStateMachine();
        }

        private void BindAssetProvider() =>
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();

        private void BindStaticDataService() =>
            Container.BindInterfacesTo<StaticDataService>().AsSingle();

        private void BindSceneLoader() =>
            Container.BindInterfacesAndSelfTo<SceneLoader.SceneLoader>().AsSingle();

        private void BindGameStateMachine() =>
            Container.Bind<GlobalStateMachine>().AsSingle();

        private void BindStatesFactory() =>
            Container.Bind<StatesFactory>().AsSingle();
    }
}