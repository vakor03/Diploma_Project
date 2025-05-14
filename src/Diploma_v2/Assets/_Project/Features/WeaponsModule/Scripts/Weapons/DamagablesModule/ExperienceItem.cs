using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    [CreateAssetMenu(menuName = "Configurations/Items/CreateNewItem", fileName = "ExperienceItem", order = 0)]
    public class ExperienceItem : Item {
        [SerializeField] private int _experienceAmount;

        public override void PerformPickupOperation(GameObject picker) {
            Debug.Log("Performing pickup operation!");
            if (picker.TryGetComponent(out IEntityExperience entityExperience))
                entityExperience.AddExperience(_experienceAmount);
        }
    }
}