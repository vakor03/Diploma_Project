using Zenject;

namespace _Project.Features.CameraShakeModule.Scripts {
	public class CameraShakeInstaller : MonoInstaller {
		public override void InstallBindings() =>
			Container.Bind<ICameraShakeService>().To<CameraShakeService>().FromComponentsInHierarchy().AsSingle();
	}
}