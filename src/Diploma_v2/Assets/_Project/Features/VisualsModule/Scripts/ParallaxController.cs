using _Project.Features.CameraModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.VisualsModule.Scripts {
    public class ParallaxController : MonoBehaviour {
        [SerializeField] ParallaxLayer[] _backgrounds;
        [SerializeField] private bool _isWithXParallax = true;
        [SerializeField] private bool _isWithYParallax = true;

        private CameraDataHolder _cameraDataHolder;

        private Transform _cameraTransform;
        private Vector3 _previousCamPos;

        [Inject]
        private void InjectDependencies(CameraDataHolder cameraDataHolder) =>
            _cameraDataHolder = cameraDataHolder;

        private void Awake() =>
            _cameraTransform = _cameraDataHolder.Camera.transform;

        private void Start() =>
            _previousCamPos = _cameraTransform.position;

        private bool _isCameraInitialized = false;

        private void Update() {
            if (!_isCameraInitialized) {
                if (_cameraTransform.position != Vector3.zero) {
                    _previousCamPos = transform.position;
                    _isCameraInitialized = true;
                }
                else
                    return;
            }

            for (int i = 0; i < _backgrounds.Length; i++) {
                ParallaxLayer parallaxLayer = _backgrounds[i];
                float parallaxY = 0;
                if(_isWithYParallax)
                    parallaxY = (_previousCamPos.y - _cameraTransform.position.y) * -parallaxLayer.Multiplier;
                float parallaxX = 0;
                if(_isWithXParallax)
                    parallaxX = (_previousCamPos.x - _cameraTransform.position.x) * -parallaxLayer.Multiplier;

                parallaxLayer.Transform.position = new Vector3(parallaxLayer.Transform.position.x + parallaxX, parallaxLayer.Transform.position.y + parallaxY,
                    parallaxLayer.Transform.position.z);
            }

            _previousCamPos = _cameraTransform.position;
        }
    }
}