using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations {
	[CreateAssetMenu(fileName = nameof(WeaponsConfigurationHolder) + "_Default",
		menuName = "Configurations/WeaponsModule/" + nameof(WeaponsConfigurationHolder))]
	public class WeaponsConfigurationHolder : ScriptableObject {
		[SerializeField] private SerializedDictionary<WeaponType, WeaponConfiguration> _weaponConfigurations;
		
		public WeaponConfiguration GetConfiguration(WeaponType weaponType) =>
			_weaponConfigurations[weaponType];
		
		public bool IsConfigurationExists(WeaponType weaponType) =>
			_weaponConfigurations.ContainsKey(weaponType);
	}
}