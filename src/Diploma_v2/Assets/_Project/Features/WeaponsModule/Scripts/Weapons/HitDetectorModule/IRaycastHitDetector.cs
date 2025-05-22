using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.HitDetectorModule {
	public interface IRaycastHitDetector {
		public Hit DetectHit(Vector2 startPosition, Vector2 direction, float maxDistance, LayerMask enemyLayerMask);
	}
}