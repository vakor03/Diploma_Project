using System;
using _Project.Features.StatsModule;
using _Project.Features.WeaponModule;
using _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule;
using _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesPool;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponSpreadModule;
using Features.WeaponsModule.Scripts.Weapons.WeaponsInstances;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using IShootable = _Project.Features.WeaponModule.IShootable;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	public class GuardRobotWeapon : MonoBehaviour, IWeapon, IShootable, IReloadable, IRotatable {
		[SerializeField] private Transform _firePoint;
		[SerializeField] private Vector2 _attackDirection;

		private const WeaponType WEAPON_TYPE = WeaponType.GuardRobotWeapon;
		private const ProjectileType PROJECTILE_TYPE = ProjectileType.BulletRaycast;

		private ProjectilesObjectPool _projectilesPool;
		private WeaponConfiguration _weaponConfiguration;
		private IWeaponConfigurationService _weaponConfigurationService;
		private IWeaponSpreadService _weaponSpreadService;
		private LayersConfiguration _layersConfiguration;

		private int _bulletsInMagazine;
		private float _lastShotTime;
		private float _reloadingTimer;

		[Inject]
		private void InjectDependencies(ProjectilesObjectPool projectilePoolService,
		                                IWeaponSpreadService weaponSpreadService,
		                                IWeaponConfigurationService weaponConfigurationService,
		                                LayersConfiguration layersConfiguration) {
			_projectilesPool = projectilePoolService;
			_weaponSpreadService = weaponSpreadService;
			_weaponConfigurationService = weaponConfigurationService;
			_layersConfiguration = layersConfiguration;
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

		[Button]
		public void Shoot(Vector2 direction) {
			if (IsReloading || Time.time - _lastShotTime < _weaponConfiguration.ShootDelay)
				return;

			IProjectile projectile = _projectilesPool.Get(PROJECTILE_TYPE);
			projectile.GetBehaviour<IRaycastBulletBehaviour>()
			          .SetStartPosition(_firePoint.position)
			          .SetDirection(_weaponSpreadService.ApplySpread(direction, _weaponConfiguration.Spread))
			          .SetDamage(_weaponConfiguration.Damage)
			          .SetSpeed(_weaponConfiguration.Speed)
			          .SetMaxDistance(_weaponConfiguration.MaxDistance)
			          .SetEnemyLayerMask(_layersConfiguration.PlayerLayerMask)
			          .SetTrailRendererConfiguration(_weaponConfiguration.TrailRendererConfiguration);

			projectile.Launch();

			_lastShotTime = Time.time;
			if (--_bulletsInMagazine == 0)
				StartReloading();
		}

		public void Shoot() =>
			Shoot(GetAttackDirection());

		public bool IsReloading => _reloadingTimer > 0;

		public void StartReloading() =>
			_reloadingTimer = _weaponConfiguration.ReloadTime;

		public void RotateInDirection(Vector2 direction)
		{
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    
			if (direction.x < 0)
			{
				transform.localScale = new Vector3(-1, 1, 1);
				angle -= 180;
			}
			else
				transform.localScale = new Vector3(1, 1, 1);

			transform.rotation = Quaternion.Euler(0f, 0f, angle);
		}

		private void RefillMagazine() =>
			_bulletsInMagazine = _weaponConfiguration.MagazineSize;

		public IStatService<WeaponStats> Stats { get; private set; }

		public void InitWeaponStats(IStatService<WeaponStats> weaponStats) {
			Stats = weaponStats;
		}

		private Vector3 GetAttackDirection() =>
			transform.rotation * (_attackDirection * transform.localScale.x);
	}
} 