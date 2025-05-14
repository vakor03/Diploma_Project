using _Project.Features.WeaponsModule.Scripts.Muzzles.MuzzlesPoolModule;
using Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule;
using UnityEngine;
using Zenject;

namespace Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	public class ExplodeOnHit : MonoBehaviour {
		[SerializeField] private MuzzleType _muzzleType;

		private IProjectileBehaviour _projectileBehaviour;
		private MuzzlesPool _muzzlesPool;

		[Inject]
		public void InjectDependencies(MuzzlesPool muzzlesPool) =>
			_muzzlesPool = muzzlesPool;

		private void Awake() =>
			_projectileBehaviour = GetComponent<IProjectileBehaviour>();

		private void OnEnable() =>
			_projectileBehaviour.OnHit += OnHit;

		private void OnDisable() =>
			_projectileBehaviour.OnHit -= OnHit;

		private void OnHit(Vector2 contactPoint) {
			MonoPooledMuzzle redMuzzle = _muzzlesPool.Get(_muzzleType);
			redMuzzle.transform.position = contactPoint;
		}
	}
}