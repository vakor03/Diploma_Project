using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Features.CameraShakeModule.Scripts {
	public class CameraShakeService : MonoBehaviour, ICameraShakeService {
		private CinemachineCamera _virtualCamera;
		private CinemachineBasicMultiChannelPerlin _cinemachinePerlin;

		private float _shakeTimer;
		private float _shakeTimerTotal;
		private float _startingIntensity;

		private void Awake() =>
			_cinemachinePerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();

		public void ShakeCamera(float intensity, float duration) {
			_cinemachinePerlin.AmplitudeGain = intensity;
			_startingIntensity = intensity;
			_shakeTimer = duration;
			_shakeTimerTotal = duration;
		}

		public void ShakeCamera(CameraShakeConfiguration cameraShakeConfiguration) =>
			ShakeCamera(cameraShakeConfiguration.intensity, cameraShakeConfiguration.duration);

		public void Update() {
			if (_shakeTimer < 0)
				return;

			_shakeTimer -= Time.deltaTime;
			_cinemachinePerlin.AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, 1 - _shakeTimer / _shakeTimerTotal);
		}
	}
}