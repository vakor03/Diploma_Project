using _Project.Features.StatsModule;
using _Project.Features.WeaponModule;
using _Project.Features.WeaponsModule.Scripts.Muzzles.MuzzlesPoolModule;
using _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule;
using _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesPool;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using EasyButtons;
using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	public class AK47 : MonoBehaviour, IWeapon, IDirectionalShootable, IReloadable {
		[SerializeField] private Transform _firePoint;

		private const ProjectileType PROJECTILE_TYPE = ProjectileType.Bullet;
		private const WeaponType WEAPON_TYPE = WeaponType.AK47;
		private const MuzzleType MUZZLE_TYPE = MuzzleType.RedMuzzles;

		private float _reloadingTimer;
		private IWeaponConfigurationService _weaponConfigurationService;
		private WeaponConfiguration _weaponConfiguration;
		private ProjectilesObjectPool _projectilesPool;

		[Inject]
		private void InjectDependencies(IWeaponConfigurationService weaponConfigurationService,
		                                ProjectilesObjectPool projectilePoolService) {
			_projectilesPool = projectilePoolService;
			_weaponConfigurationService = weaponConfigurationService;
		}

		private void Awake() =>
			_weaponConfiguration = _weaponConfigurationService.GetConfiguration(WEAPON_TYPE);

		private void Update() {
			if (IsReloading) {
				_reloadingTimer -= Time.deltaTime;
				if (_reloadingTimer <= 0)
					IsReloading = false;
			}
		}

		[Button]
		public void Shoot(Vector2 direction) {
			if (IsReloading)
				return;

			LaunchProjectile(direction);

			StartReloading();
		}

		public bool IsReloading { get; private set; }

		public void StartReloading() {
			IsReloading = true;
			_reloadingTimer = _weaponConfiguration.ReloadTime;
		}

		private void LaunchProjectile(Vector2 direction) {
			IProjectile projectile = _projectilesPool.Get(PROJECTILE_TYPE);
			projectile.GetBehaviour<IBulletProjectileBehaviour>()
			          .SetDirection(direction)
			          .SetStartPosition(_firePoint.position)
			          .SetSpeed(_weaponConfiguration.Speed)
			          .SetDamage(_weaponConfiguration.Damage);

			projectile.Launch();

			// _muzzlesPool.Get(MUZZLE_TYPE)
			// .With(muzzle => muzzle.SetPosition(_firePoint.position))
			// .With(muzzle => muzzle.SetParent(_firePoint))
			// .With(muzzle => muzzle.SetDirection(direction));
		}
		public IStatService<WeaponStats> Stats { get; private set; }
		public void InitWeaponStats(IStatService<WeaponStats> weaponStats) {
			Stats = weaponStats;
		}
	}
}