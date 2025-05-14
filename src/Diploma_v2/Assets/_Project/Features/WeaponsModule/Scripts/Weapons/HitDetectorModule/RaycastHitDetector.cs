using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;
using Features.WeaponsModule.Scripts.Weapons.WeaponsInstances;
using Global.Helpers.Scripts;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.HitDetectorModule {
	public class RaycastHitDetector : IRaycastHitDetector {
		private readonly LayersConfiguration _layersConfiguration;

		public RaycastHitDetector(LayersConfiguration layersConfiguration) =>
			_layersConfiguration = layersConfiguration;

		public Hit DetectHit(Vector2 startPosition, Vector2 direction, float maxDistance) {
			RaycastHit2D hit;
			hit = Physics2D.Raycast(startPosition, direction, maxDistance, _layersConfiguration.EnemyLayerMask);
			if (hit.collider != null)
				return new TargetHit()
				       .With(targetHit => targetHit.Point = hit.point)
				       .With(targetHit => targetHit.Distance = hit.distance)
				       .With(targetHit => targetHit.Damageable = hit.collider.GetComponent<IDamageable>());

			hit = Physics2D.Raycast(startPosition, direction, maxDistance, _layersConfiguration.EnvironmentLayerMask);
			if (hit.collider != null)
				return new EnvironmentHit()
				       .With(environmentHit => environmentHit.Point = hit.point)
				       .With(environmentHit => environmentHit.Distance = hit.distance);

			return new NullHit()
			       .With(nullHit => nullHit.Point = startPosition + direction * maxDistance)
			       .With(nullHit => nullHit.Distance = maxDistance);
		}
	}
}