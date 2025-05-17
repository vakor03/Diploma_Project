using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;
using Zenject;

namespace _Project.Features.ExperienceModule.Orbs {
    public class XPOrbFactory : IXPOrbFactory {
        private readonly XPOrbConfiguration _orbConfig;
        private readonly IInstantiator _instantiator;

        public XPOrbFactory(IStaticDataService staticDataService, DiContainer instantiator) {
            _orbConfig = staticDataService.GetXPOrbConfiguration();
            _instantiator = instantiator;
        }

        public XPOrb CreateXPOrb(XPOrbType orbType, float experience, Vector3 position) {
            XPOrbData orbData = _orbConfig.GetOrbDataByType(orbType);

            XPOrb orb = _instantiator.InstantiatePrefabForComponent<XPOrb>(orbData.prefab, position, Quaternion.identity, null);
            orb.Initialize(experience, orbData);

            return orb;
        }
    }
}