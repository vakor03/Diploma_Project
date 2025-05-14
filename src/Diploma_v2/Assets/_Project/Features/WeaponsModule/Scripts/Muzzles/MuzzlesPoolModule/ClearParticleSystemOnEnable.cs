using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Muzzles.MuzzlesPoolModule {
	public class ClearParticleSystemOnEnable : MonoBehaviour {
		private ParticleSystem _particleSystem;
		
		private void Awake() =>
			_particleSystem = GetComponent<ParticleSystem>();
		
		private void OnEnable() {
			_particleSystem.Stop();
			_particleSystem.Clear();
			_particleSystem.Play();
		}
	}
}