namespace _Project.Features.CameraShakeModule.Scripts {
	public interface ICameraShakeService {
		public void ShakeCamera(float intensity, float duration);
		public void ShakeCamera(CameraShakeConfiguration cameraShakeConfiguration);
	}
}