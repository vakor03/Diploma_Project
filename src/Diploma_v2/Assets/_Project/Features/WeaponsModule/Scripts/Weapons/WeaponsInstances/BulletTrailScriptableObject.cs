using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	public class BulletTrailScriptableObject : ScriptableObject {
		public AnimationCurve widthCurve;
		public float time = 0.5f;
		public Gradient colorGradient;
		public Material material;
		public int cornerVertices;
		public int endCapVertices;

		public void SetupTrail(TrailRenderer trailRenderer) {
			trailRenderer.widthCurve = widthCurve;
			trailRenderer.time = time;
			trailRenderer.colorGradient = colorGradient;
			trailRenderer.sharedMaterial = material;
			trailRenderer.numCornerVertices = cornerVertices;
			trailRenderer.numCapVertices = endCapVertices;
		}
	}
}