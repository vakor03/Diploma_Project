using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Features.StatsModule {
    [CreateAssetMenu(fileName = nameof(DefaultEnemyStats) + "_Default", menuName = "Configurations/StatsModule/" + nameof(DefaultEnemyStats))]
    public class DefaultEnemyStats : ScriptableObject
    {
        public SerializedDictionary<EnemyStats, float> stats = new();
    }
}