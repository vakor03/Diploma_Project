using Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations {
	public interface IWeaponConfigurationService {
		public WeaponConfiguration GetConfiguration(WeaponType weaponType);
	}
}