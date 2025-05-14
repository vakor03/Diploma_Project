using UnityEngine;

namespace Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule {
	public interface IBulletProjectileBehaviour : IProjectileBehaviour {
		public IBulletProjectileBehaviour SetDirection(Vector2 direction);
		public IBulletProjectileBehaviour SetSpeed(float speed);
		public IBulletProjectileBehaviour SetDamage(float damage);
		public IBulletProjectileBehaviour SetStartPosition(Vector2 position);
	}
}