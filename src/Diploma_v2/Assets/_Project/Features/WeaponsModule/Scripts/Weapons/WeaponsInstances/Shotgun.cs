using _Project.Features.StatsModule;
using _Project.Features.WeaponModule;
using _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesPool;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponSpreadModule;
using Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule;
using Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using UnityEngine;
using Zenject;

namespace Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	public class Shotgun : MonoBehaviour, IWeapon, IReloadable, IDirectionalShootable {
		[SerializeField] private Transform _firePoint;

		private const WeaponType WEAPON_TYPE = WeaponType.Shotgun;
		private const ProjectileType PROJECTILE_TYPE = ProjectileType.BulletRaycast;

		private ProjectilesObjectPool _projectilesPool;
		private IWeaponConfigurationService _weaponConfigurationService;
		private IWeaponSpreadService _weaponSpreadService;

		private float _reloadingTimer;

		private WeaponConfiguration _weaponConfiguration;

		[Inject]
		private void InjectDependencies(ProjectilesObjectPool projectilePoolService,
		                                IWeaponConfigurationService weaponConfigurationService,
		                                IWeaponSpreadService weaponSpreadService) {
			_projectilesPool = projectilePoolService;
			_weaponConfigurationService = weaponConfigurationService;
			_weaponSpreadService = weaponSpreadService;
		}

		private void Awake() =>
			_weaponConfiguration = _weaponConfigurationService.GetConfiguration(WEAPON_TYPE);

		public void Shoot(Vector2 direction) {
			if (IsReloading)
				return;

			LaunchProjectiles(direction);

			StartReloading();
		}
		public IStatService<WeaponStats> Stats { get; private set; }
		public void InitWeaponStats(IStatService<WeaponStats> weaponStats) {
			Stats = weaponStats;
		}
		public bool IsReloading => _reloadingTimer > 0;

		public void StartReloading() =>
			_reloadingTimer = _weaponConfiguration.ReloadTime;

		private void Update() {
			if (IsReloading)
				_reloadingTimer -= Time.deltaTime;
		}

		private void LaunchProjectiles(Vector2 direction) {
			for (int i = 0; i < _weaponConfiguration.BurstCount; i++)
				LaunchProjectile(direction);
		}

		private void LaunchProjectile(Vector2 direction) {
			IProjectile projectile = _projectilesPool.Get(PROJECTILE_TYPE);

			projectile.GetBehaviour<IRaycastBulletBehaviour>()
			          .SetDirection(_weaponSpreadService.ApplySpread(direction, _weaponConfiguration.Spread))
			          .SetDamage(_weaponConfiguration.Damage)
			          .SetSpeed(_weaponConfiguration.Speed)
			          .SetStartPosition(_firePoint.position)
			          .SetMaxDistance(_weaponConfiguration.MaxDistance)
			          .SetTrailRendererConfiguration(_weaponConfiguration.TrailRendererConfiguration);

			projectile.Launch();
		}
	}
}