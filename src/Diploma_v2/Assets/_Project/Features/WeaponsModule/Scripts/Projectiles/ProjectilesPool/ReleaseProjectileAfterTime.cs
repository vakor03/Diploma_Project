using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesPool {
	public class ReleaseProjectileAfterTime : MonoBehaviour {
		[SerializeField] private float _timeToRelease = 5f;
		[SerializeField] private MonoPooledProjectile _monoPooledProjectile;

		private ProjectilesObjectPool _projectilesPool;

		[Inject]
		public void InjectDependencies(ProjectilesObjectPool projectilesPool) =>
			_projectilesPool = projectilesPool;

		private void OnEnable() =>
			Invoke(nameof(Release), _timeToRelease);

		private void Release() =>
			_projectilesPool.Release(_monoPooledProjectile);
	}
}