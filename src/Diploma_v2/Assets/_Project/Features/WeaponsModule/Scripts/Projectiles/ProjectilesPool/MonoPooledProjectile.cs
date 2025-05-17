using System.Linq;
using _Project.Features.ObjectPoolModule;
using _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesPool {
	public class MonoPooledProjectile : MonoBehaviour, IPooledObject<ProjectileType>, IProjectile {
		private ProjectilesObjectPool _projectilesPool;
		private IProjectileBehaviour[] _projectileBehaviours;

		[Inject]
		public void InjectDependencies(ProjectilesObjectPool projectilesPool) =>
			_projectilesPool = projectilesPool;

		public ProjectileType Type { get; set; }

		private void Awake() =>
			_projectileBehaviours = GetComponents<IProjectileBehaviour>();

		private void OnEnable() {
			foreach (IProjectileBehaviour projectileBehaviour in _projectileBehaviours)
				projectileBehaviour.OnDestroy += ReturnToPool;
		}

		private void OnDisable() {
			foreach (IProjectileBehaviour projectileBehaviour in _projectileBehaviours)
				projectileBehaviour.OnDestroy -= ReturnToPool;
		}

		private void ReturnToPool() =>
			_projectilesPool.Release(this);

		public void OnEnabled() =>
			gameObject.SetActive(true);

		public void OnDisabled() =>
			gameObject.SetActive(false);

		public void OnDestroyed() {
			if (gameObject == null)
				return;
			Destroy(gameObject);
		}

		public void Launch() {
			foreach (IProjectileBehaviour projectileBehaviour in _projectileBehaviours)
				projectileBehaviour.Activate();
		}

		public TProjectileBehaviour GetBehaviour<TProjectileBehaviour>()
			where TProjectileBehaviour : IProjectileBehaviour =>
			(TProjectileBehaviour)_projectileBehaviours.First(behaviours => behaviours is TProjectileBehaviour);
	}
}