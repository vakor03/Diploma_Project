using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    [CreateAssetMenu(fileName = nameof(ExperienceItemsConfiguration) + "_Default",
        menuName = "Configurations/Items/" + nameof(ExperienceItemsConfiguration))]
    public class ExperienceItemsConfiguration : ScriptableObject {
        [field: SerializeField] public SerializedDictionary<int, PickupItem> ExperienceItems { get; private set; }

        public List<int> GetExperienceValuesSortedInDescending() {
            if (_experienceValuesSortedInDescending == null || _experienceValuesSortedInDescending.Count == 0)
                _experienceValuesSortedInDescending = ExperienceItems.Keys.OrderByDescending(value=>value).ToList();
            return _experienceValuesSortedInDescending;
        }

        private List<int> _experienceValuesSortedInDescending = null;
    }
}