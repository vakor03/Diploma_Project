using Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations {
	public class WeaponConfigurationService : IWeaponConfigurationService {
		private readonly WeaponsConfigurationHolder _weaponsConfigurationHolder;
		
		public WeaponConfigurationService(WeaponsConfigurationHolder weaponsConfigurationHolder) =>
			_weaponsConfigurationHolder = weaponsConfigurationHolder;

		public WeaponConfiguration GetConfiguration(WeaponType weaponType) {
			if (!_weaponsConfigurationHolder.IsConfigurationExists(weaponType))
				throw new($"Configuration for {weaponType} not found");

			return _weaponsConfigurationHolder.GetConfiguration(weaponType);
		}
	}
}