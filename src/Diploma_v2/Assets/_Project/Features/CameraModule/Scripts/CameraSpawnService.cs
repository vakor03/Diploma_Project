using _Project.Scripts.Infrastructure.AssetProviders;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace _Project.Features.CameraModule {
    public class CameraSpawnService : ICameraSpawnService {
        private readonly CameraDataHolder _cameraDataHolder;
        private readonly CinemachineVirtualCameraBase _camera;
        private readonly IInstantiator _instantiator;

        public CameraSpawnService(CameraDataHolder cameraDataHolder, IStaticDataService staticDataService, IInstantiator instantiator) {
            _cameraDataHolder = cameraDataHolder;
            _instantiator = instantiator;
            _camera = staticDataService.GetCameraPrefab();
        }

        public void SpawnCamera() {
            if (!_camera)
                Debug.LogError("Camera prefab is not set in the static data service.");
            else {
                _cameraDataHolder.Camera = _instantiator.InstantiatePrefabForComponent<CinemachineVirtualCameraBase>(_camera);
                _cameraDataHolder.Camera.gameObject.SetActive(true);
            }
        }
    }
}