using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponSpreadModule {
	public class WeaponSpreadServiceInstaller : Installer<WeaponSpreadServiceInstaller> {
		public override void InstallBindings() =>
			Container.Bind<IWeaponSpreadService>().To<WeaponSpreadService>().AsSingle();
	}
}