using UnityEngine;

namespace _Project.Features.ExperienceModule {
    public interface IXPOrbFactory {
        public XPOrb CreateXPOrb(XPOrbType orbType, float experience, Vector3 position);
    }
}