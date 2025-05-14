using _Project.Features.CameraShakeModule.Scripts;
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
	public class Minigun : MonoBehaviour, IWeapon, IDirectionalShootable, IReloadable {
		[SerializeField] private BoxCollider2D _firePointBounds;

		private const WeaponType WEAPON_TYPE = WeaponType.Minigun;
		private const ProjectileType PROJECTILE_TYPE = ProjectileType.BulletRaycast;

		private ProjectilesObjectPool _projectilesPool;
		private WeaponConfiguration _weaponConfiguration;
		private IWeaponConfigurationService _weaponConfigurationService;
		private IWeaponSpreadService _weaponSpreadService;
		private ICameraShakeService _cameraShakeService;

		private int _bulletsInMagazine;
		private float _lastShotTime;
		private float _reloadingTimer;

		[Inject]
		private void InjectDependencies(ProjectilesObjectPool projectilePoolService,
		                                IWeaponSpreadService weaponSpreadService,
		                                IWeaponConfigurationService weaponConfigurationService,
		                                ICameraShakeService cameraShakeService) {
			_projectilesPool = projectilePoolService;
			_weaponSpreadService = weaponSpreadService;
			_weaponConfigurationService = weaponConfigurationService;
			_cameraShakeService = cameraShakeService;
		}

		private void Awake() {
			_weaponConfiguration = _weaponConfigurationService.GetConfiguration(WEAPON_TYPE);
			RefillMagazine();
		}

		private void Update() {
			if (!IsReloading)
				return;

			_reloadingTimer -= Time.deltaTime;
			if (_reloadingTimer <= 0)
				RefillMagazine();
		}

		public void Shoot(Vector2 direction) {
			if (IsReloading || Time.time - _lastShotTime < _weaponConfiguration.ShootDelay)
				return;

			LaunchProjectile(direction);

			_cameraShakeService.ShakeCamera(_weaponConfiguration.CameraShakeConfiguration);

			_lastShotTime = Time.time;
			if (--_bulletsInMagazine == 0)
				StartReloading();
		}

		public bool IsReloading => _reloadingTimer > 0;

		public void StartReloading() =>
			_reloadingTimer = _weaponConfiguration.ReloadTime;

		private void RefillMagazine() =>
			_bulletsInMagazine = _weaponConfiguration.MagazineSize;

		private void LaunchProjectile(Vector2 direction) {
			IProjectile projectile = _projectilesPool.Get(PROJECTILE_TYPE);
			projectile.GetBehaviour<IRaycastBulletBehaviour>()
			          .SetStartPosition(GetRandomFirePoint())
			          .SetDirection(_weaponSpreadService.ApplySpread(direction, _weaponConfiguration.Spread))
			          .SetDamage(_weaponConfiguration.Damage)
			          .SetSpeed(_weaponConfiguration.Speed)
			          .SetMaxDistance(_weaponConfiguration.MaxDistance)
			          .SetTrailRendererConfiguration(_weaponConfiguration.TrailRendererConfiguration);

			projectile.Launch();
		}

		private Vector2 GetRandomFirePoint() {
			Vector2 min = _firePointBounds.bounds.min;
			Vector2 max = _firePointBounds.bounds.max;

			return new(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
		}

		public IStatService<WeaponStats> Stats { get; private set; }
		public void InitWeaponStats(IStatService<WeaponStats> weaponStats) {
			Stats = weaponStats;
		}
	}
}