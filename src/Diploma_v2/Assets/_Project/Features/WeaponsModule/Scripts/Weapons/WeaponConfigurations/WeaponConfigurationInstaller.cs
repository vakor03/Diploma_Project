using _Project.Extensions.ZenjectExtensions;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations {
	public class WeaponConfigurationInstaller : Installer<WeaponConfigurationInstaller> {
		public override void InstallBindings() {
			BindWeaponConfigurationHolder();

			BindWeaponConfigurationService();
		}

		private void BindWeaponConfigurationHolder() =>
			Container.BindConfigurationFromAddressables<WeaponsConfigurationHolder>(AssetPath.Configuration.WEAPONS_CONFIGURATIONS_HOLDER)
			         .AsSingle();
		
		private void BindWeaponConfigurationService() =>
			Container.BindInterfacesTo<WeaponConfigurationService>()
			         .AsSingle();
	}
}