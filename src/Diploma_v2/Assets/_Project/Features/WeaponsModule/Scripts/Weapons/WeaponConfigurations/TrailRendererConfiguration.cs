using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations {
	[CreateAssetMenu(menuName = "Configurations/Other/" + nameof(TrailRendererConfiguration), fileName = nameof(TrailRendererConfiguration) + "_Default", order = 0)]
	public class TrailRendererConfiguration : ScriptableObject {
		public float time;
		public AnimationCurve widthCurve;
		public Gradient colorGradient;
		public Material material;

		public void ApplyTo(TrailRenderer trailRenderer) {
			trailRenderer.time = time;
			trailRenderer.widthCurve = widthCurve;
			trailRenderer.colorGradient = colorGradient;
			trailRenderer.sharedMaterial = material;
		}
	}
}