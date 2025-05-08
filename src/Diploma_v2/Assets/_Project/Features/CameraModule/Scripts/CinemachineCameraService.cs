using UnityEngine;

namespace _Project.Features.CameraModule {
    public class CinemachineCameraService : ICameraService {
        private readonly CameraDataHolder _cameraDataHolder;

        public CinemachineCameraService(CameraDataHolder cameraDataHolder) =>
            _cameraDataHolder = cameraDataHolder;

        public void FollowTarget(Transform target) =>
            _cameraDataHolder.Camera.Follow = target;
    }
}