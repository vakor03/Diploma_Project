using _Project.Features.CameraShakeModule.Scripts;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponConfigurations {
	[CreateAssetMenu(fileName = nameof(WeaponConfiguration) + "_Default",
		menuName = "Configurations/WeaponsModule/" + nameof(WeaponConfiguration))]
	public class WeaponConfiguration : ScriptableObject {
		[SerializeField] private float _damage = 2f;
		[SerializeField] private float _speed = 10f;
		[SerializeField] private float _reloadTime = .5f;
		[SerializeField] private float _spread = .05f;
		[SerializeField] private float _maxDistance = 100f;
		[SerializeField] private TrailRendererConfiguration _trailRendererConfiguration;
		[SerializeField] private int _burstCount = 1;
		[SerializeField] private int _magazineSize = 30;
		[SerializeField] private float _fireRate = 0.1f;
		[SerializeField] private CameraShakeConfiguration _cameraShakeConfiguration;
		[SerializeField] private ParticleSystem.MinMaxCurve _damageButCurve; // TODO: Implement this

		public float Damage => _damage;
		public float Speed => _speed;
		public float ReloadTime => _reloadTime;
		public float Spread => _spread;
		public float MaxDistance => _maxDistance;
		public TrailRendererConfiguration TrailRendererConfiguration => _trailRendererConfiguration;
		public int BurstCount => _burstCount;
		public float ShootDelay => 1 / _fireRate;
		public int MagazineSize => _magazineSize;
		public CameraShakeConfiguration CameraShakeConfiguration => _cameraShakeConfiguration;
	}
}