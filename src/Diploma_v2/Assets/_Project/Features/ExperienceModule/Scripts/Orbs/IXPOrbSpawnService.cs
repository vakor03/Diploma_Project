using UnityEngine;

namespace _Project.Features.ExperienceModule {
    public interface IXPOrbSpawnService {
        public void SpawnXPOrb(float experience, Vector3 position);
    }
}