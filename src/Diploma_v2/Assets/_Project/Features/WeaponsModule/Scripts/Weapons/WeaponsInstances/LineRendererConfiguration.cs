using UnityEngine;

namespace Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	[CreateAssetMenu(menuName = "Configurations/Other/" + nameof(LineRendererConfiguration),
		fileName = nameof(LineRendererConfiguration) + "_Default", order = 0)]
	public class LineRendererConfiguration : ScriptableObject {
		[SerializeField] private Material _material;
		[SerializeField] private AnimationCurve _widthCurve;
		[SerializeField] private Gradient _colorGradient;

		public void ApplyTo(LineRenderer lineRenderer) {
			lineRenderer.material = _material;
			lineRenderer.widthCurve = _widthCurve;
			lineRenderer.colorGradient = _colorGradient;
		}
	}
}