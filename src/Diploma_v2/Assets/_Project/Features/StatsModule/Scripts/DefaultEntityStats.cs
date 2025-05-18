using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Features.StatsModule {
    [CreateAssetMenu(fileName = nameof(DefaultEntityStats) + "_Default", menuName = "Configurations/StatsModule/" + nameof(DefaultEntityStats))]
    public class DefaultEntityStats : ScriptableObject
    {
        public SerializedDictionary<EntityStats, float> stats = new();
    }
}