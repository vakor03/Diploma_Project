using System;
using System.Collections;
using _Project.Features.WeaponsModule.Scripts.Weapons.HitDetectorModule;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations;
using Global.Helpers.Scripts;
using UnityEngine;
using Zenject;

namespace Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	public class RaycastBulletProjectileBehaviour : MonoBehaviour, IRaycastBulletBehaviour, IHitVisitor {
		[SerializeField] private TrailRenderer _trailRenderer;
		[SerializeField] private LayerMask _enemyLayerMask;

		private float _damage;
		private Vector2 _direction;
		private float _maxDistance;
		private float _speed;
		private Vector2 _startPosition;

		private IRaycastHitDetector _raycastHitDetector;
		
		public event Action<Vector2> OnHit;
		public event Action OnDestroy;

		[Inject]
		private void InjectDependencies(IRaycastHitDetector raycastHitDetector) =>
			_raycastHitDetector = raycastHitDetector;

		public void Visit(Hit hit) =>
			StartCoroutine(SpawnTrail(_trailRenderer, hit.Point));

		public void Visit(TargetHit hit) {
			StartCoroutine(InvokeOnHitAfterTime(hit.Point, hit.Distance / _speed));
			hit.Damageable.TakeDamage(_damage);
		}

		public void Visit(EnvironmentHit hit) =>
			StartCoroutine(InvokeOnHitAfterTime(hit.Point, hit.Distance / _speed));

		public void Visit(NullHit hit) { }

		public void Activate() {
			StartCoroutine(ResetTrailRenderer());

			transform.position = _startPosition;

			_raycastHitDetector.DetectHit(_startPosition, _direction, _maxDistance).Accept(this);
		}

		private IEnumerator InvokeOnHitAfterTime(Vector2 hitPoint, float timeToReach) {
			yield return new WaitForSeconds(timeToReach);
			OnHit?.Invoke(hitPoint);
		}

		private IEnumerator SpawnTrail(TrailRenderer trailRenderer, Vector2 finalPosition) {
			float time = 0;
			Vector2 startPosition = transform.position;

			float totalDistance = Vector2.Distance(startPosition, finalPosition);
			float timeToReach = totalDistance / _speed;

			while (time < timeToReach) {
				transform.position = Vector2.Lerp(startPosition, finalPosition, time / timeToReach);
				time += Time.deltaTime;

				yield return null;
			}

			transform.position = finalPosition;

			Invoke(nameof(InvokeOnDestroy), trailRenderer.time);
		}

		private IEnumerator ResetTrailRenderer() {
			_trailRenderer.Clear();
			_trailRenderer.emitting = false;
			yield return null;
			_trailRenderer.emitting = true;
		}

		private void InvokeOnDestroy() =>
			OnDestroy?.Invoke();

		#region Builder

		public IRaycastBulletBehaviour SetDirection(Vector2 direction) =>
			this.With(() => _direction = direction.normalized);

		public IRaycastBulletBehaviour SetStartPosition(Vector2 startPosition) =>
			this.With(() => _startPosition = startPosition);

		public IRaycastBulletBehaviour SetDamage(float damage) =>
			this.With(() => _damage = damage);

		public IRaycastBulletBehaviour SetSpeed(float speed) =>
			this.With(() => _speed = speed);

		public IRaycastBulletBehaviour SetMaxDistance(float maxDistance) =>
			this.With(() => _maxDistance = maxDistance);

		public IRaycastBulletBehaviour SetTrailRendererConfiguration(
			TrailRendererConfiguration trailRendererConfiguration) {
			trailRendererConfiguration.ApplyTo(_trailRenderer);
			return this;
		}

		#endregion
	}
}