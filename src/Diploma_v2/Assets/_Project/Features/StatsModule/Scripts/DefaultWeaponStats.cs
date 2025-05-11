using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Features.StatsModule {
    [CreateAssetMenu(fileName = nameof(DefaultWeaponStats) + "_Default", menuName = "Configurations/StatsModule/" + nameof(DefaultWeaponStats))]
    public class DefaultWeaponStats : ScriptableObject
    {
        public SerializedDictionary<WeaponStats, float> stats = new SerializedDictionary<WeaponStats, float>();
    }
}