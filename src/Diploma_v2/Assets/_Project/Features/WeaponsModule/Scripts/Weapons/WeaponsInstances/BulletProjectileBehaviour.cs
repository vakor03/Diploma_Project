using System;
using System.Collections;
using _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule;
using _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesPool;
using Global.Helpers.Scripts;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	[SelectionBase]
	public class BulletProjectileBehaviour : MonoBehaviour, IBulletProjectileBehaviour {
		[SerializeField] private MonoPooledProjectile _monoPooledProjectile;
		[SerializeField] private TrailRenderer _trailRenderer;

		private float _damage;
		private Vector2 _direction;
		private float _speed;
		private Vector2 _startPosition;

		private void Update() =>
			transform.position += (Vector3)(_direction * (_speed * Time.deltaTime));

		private void OnCollisionEnter2D(Collision2D collision) {
			int contactsCount = collision.contactCount;

			if (contactsCount == 0)
				return;

			OnHit?.Invoke(collision.GetContact(0).point);
			OnDestroy?.Invoke();
		}

		public IBulletProjectileBehaviour SetDirection(Vector2 direction) =>
			this.With(() => _direction = direction.normalized);

		public IBulletProjectileBehaviour SetSpeed(float speed) =>
			this.With(() => _speed = speed);

		public IBulletProjectileBehaviour SetDamage(float damage) =>
			this.With(() => _damage = damage);

		public IBulletProjectileBehaviour SetStartPosition(Vector2 position) =>
			this.With(() => _startPosition = position);

		public event Action<Vector2> OnHit;
		public event Action OnDestroy;

		public void Activate() {
			transform.position = _startPosition;
			float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			RestartTrailRenderer();
		}

		private void RestartTrailRenderer() =>
			StartCoroutine(StartTrailAfterOneFrame());

		private IEnumerator StartTrailAfterOneFrame() {
			_trailRenderer.Clear();
			_trailRenderer.emitting = false;
			yield return null;
			_trailRenderer.emitting = true;
		}
	}
}