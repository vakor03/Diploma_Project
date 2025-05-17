using UnityEngine;

namespace _Project.Features.ExperienceModule.Orbs {
    public interface IXPOrbSpawnService {
        public void SpawnXPOrb(float experience, Vector3 position);
    }
}