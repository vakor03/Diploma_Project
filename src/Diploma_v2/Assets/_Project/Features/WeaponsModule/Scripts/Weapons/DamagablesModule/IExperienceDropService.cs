using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public interface IExperienceDropService {
        public void DropExperience(int experienceAmount, Vector3 position);
    }
}