using _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	public interface IRaycastBulletBehaviour : IProjectileBehaviour {
		public IRaycastBulletBehaviour SetDirection(Vector2 direction);
		public IRaycastBulletBehaviour SetStartPosition(Vector2 startPosition);
		public IRaycastBulletBehaviour SetDamage(float damage);
		public IRaycastBulletBehaviour SetSpeed(float speed);
		public IRaycastBulletBehaviour SetMaxDistance(float maxDistance);

		public IRaycastBulletBehaviour SetTrailRendererConfiguration(TrailRendererConfiguration trailRendererConfiguration);
		public IRaycastBulletBehaviour SetEnemyLayerMask(LayerMask enemyLayerMask);
	}
}