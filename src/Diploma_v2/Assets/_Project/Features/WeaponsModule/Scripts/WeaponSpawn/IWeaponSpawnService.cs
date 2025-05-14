using _Project.Extensions.ZenjectExtensions;
using _Project.Features.WeaponModule;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsInstances;
using _Project.Scripts.Infrastructure.AssetProviders;
using Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.WeaponSpawn {
    public interface IWeaponSpawnService {
        public IWeapon SpawnWeapon(WeaponType weaponType, Transform parent, Vector3 position);
    }

    public class WeaponSpawnService : IWeaponSpawnService {
        private readonly WeaponInstanceConfiguration _weaponInstanceConfiguration;
        private readonly IInstantiator _instantiator;
        
        public WeaponSpawnService(WeaponInstanceConfiguration weaponInstanceConfiguration, IInstantiator instantiator) {
            _weaponInstanceConfiguration = weaponInstanceConfiguration;
            _instantiator = instantiator;
        }

        public IWeapon SpawnWeapon(WeaponType weaponType, Transform parent, Vector3 position) {
            GameObject prefab = _weaponInstanceConfiguration.WeaponPrefabs[weaponType];
            return _instantiator.InstantiatePrefabForComponent<IWeapon>(prefab, position, Quaternion.identity, parent);
        }
    }

    public class WeaponSpawnServiceInstaller : Installer<WeaponSpawnServiceInstaller> {
        public override void InstallBindings() {
            Container.BindConfigurationFromAddressables<WeaponInstanceConfiguration>(AssetPath.Configuration.WEAPON_INSTANCE_CONFIGURATION)
                .AsSingle();
            
            Container.Bind<IWeaponSpawnService>().To<WeaponSpawnService>().AsSingle();
        }
    }
}