using UnityEngine;

namespace _Project.Features.CameraModule {
    public interface ICameraService {
        public void FollowTarget(Transform target);
    }
}