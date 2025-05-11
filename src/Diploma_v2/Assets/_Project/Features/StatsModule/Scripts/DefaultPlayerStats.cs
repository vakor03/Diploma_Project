using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Features.StatsModule {
    [CreateAssetMenu(fileName = nameof(DefaultPlayerStats) + "_Default", menuName = "Configurations/StatsModule/" + nameof(DefaultPlayerStats))]
    public class DefaultPlayerStats : ScriptableObject
    {
        public SerializedDictionary<PlayerStats, float> stats = new();
    }
}