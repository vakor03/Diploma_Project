using Zenject;

namespace _Project.Features.CameraModule {
    public class CameraInstaller : Installer<CameraInstaller> {
        public override void InstallBindings() {
            Container.Bind<CameraDataHolder>().To<CameraDataHolder>().AsSingle();
            Container.Bind<ICameraService>().To<CinemachineCameraService>().AsSingle();
            Container.Bind<ICameraSpawnService>().To<CameraSpawnService>().AsSingle();
        }
    }
}