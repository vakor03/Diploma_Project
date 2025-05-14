using _Project.Features.ObjectPoolModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Muzzles.MuzzlesPoolModule {
	public class MonoPooledMuzzle : MonoBehaviour, IPooledObject<MuzzleType> {
		private MuzzlesPool _muzzlesPool;
		public MuzzleType Type { get; set; }
		
		public void SetPosition(Vector2 position) =>
			transform.position = position;
		
		public void SetParent(Transform parent) =>
			transform.SetParent(parent);
		
		[Inject]
		public void InjectDependencies(MuzzlesPool muzzlesPool) =>
			_muzzlesPool = muzzlesPool;

		public void OnEnabled() {
			gameObject.SetActive(true);
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			Invoke(nameof(ReturnToPool), 3f);
		}

		public void OnDisabled() =>
			gameObject.SetActive(false);

		public void OnDestroyed() =>
			Destroy(gameObject);

		private void ReturnToPool() =>
			_muzzlesPool.Release(this);

		public void SetDirection(Vector2 direction) {
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}
}