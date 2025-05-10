using UnityEngine;
using Zenject;

namespace _Project.Features.ExperienceModule {
    public class XPOrbFactory : IXPOrbFactory {
        private readonly XPOrbConfiguration _orbConfig;
        private readonly IInstantiator _instantiator;

        public XPOrbFactory(XPOrbConfiguration orbConfig, DiContainer instantiator) {
            _orbConfig = orbConfig;
            _instantiator = instantiator;
        }

        public XPOrb CreateXPOrb(XPOrbType orbType, float experience, Vector3 position) {
            XPOrbData orbData = _orbConfig.GetOrbDataByType(orbType);

            XPOrb orb = _instantiator.InstantiatePrefabForComponent<XPOrb>(orbData.prefab, position, Quaternion.identity, null);
            orb.Initialize(experience, orbData);

            return orb;
        }
    }
    
    public interface IXPOrbSpawnService {
        public void SpawnXPOrb(XPOrbType orbType, float experience, Vector3 position);
    }
}