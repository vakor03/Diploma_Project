using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations;
using Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule;
using UnityEngine;

namespace Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	public interface IRaycastBulletBehaviour : IProjectileBehaviour {
		public IRaycastBulletBehaviour SetDirection(Vector2 direction);
		public IRaycastBulletBehaviour SetStartPosition(Vector2 startPosition);
		public IRaycastBulletBehaviour SetDamage(float damage);
		public IRaycastBulletBehaviour SetSpeed(float speed);
		public IRaycastBulletBehaviour SetMaxDistance(float maxDistance);

		public IRaycastBulletBehaviour SetTrailRendererConfiguration(
			TrailRendererConfiguration trailRendererConfiguration);
	}
}