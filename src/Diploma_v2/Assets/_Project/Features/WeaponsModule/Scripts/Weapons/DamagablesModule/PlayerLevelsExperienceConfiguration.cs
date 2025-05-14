using System.Linq;
using _Project.Scripts.Infrastructure.AssetProviders;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    [CreateAssetMenu(fileName = nameof(PlayerLevelsExperienceConfiguration) + "_Default",
        menuName = "Configurations/Levels/" + nameof(PlayerLevelsExperienceConfiguration))]
    public class PlayerLevelsExperienceConfiguration : ScriptableObject {
        [SerializeField] private SerializedDictionary<int, int> _predefinedExperienceValues;
        [SerializeField] private int _step;

        public int GetExperienceForLevel(int level) {
            int lastExperienceValue = _predefinedExperienceValues.Keys.Max();
            if (level <= lastExperienceValue)
                return _predefinedExperienceValues[level];
            
            return _predefinedExperienceValues[lastExperienceValue] + (level - lastExperienceValue) * _step;
        }
    }
}