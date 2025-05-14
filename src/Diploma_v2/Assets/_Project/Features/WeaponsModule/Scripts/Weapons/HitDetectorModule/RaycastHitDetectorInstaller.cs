using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.HitDetectorModule {
	public class RaycastHitDetectorInstaller : Installer<RaycastHitDetectorInstaller> {
		public override void InstallBindings() =>
			Container.Bind<IRaycastHitDetector>().To<RaycastHitDetector>().AsSingle();
	}
}