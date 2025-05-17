using _Project.Features.EnemyModule;
using _Project.Features.UpgradesModule.API;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Features.StatsModule {
    [CreateAssetMenu(fileName = nameof(DefaultStatsDatabase) + "_Default", menuName = "Configurations/StatsModule/" + nameof(DefaultStatsDatabase))]
    public class DefaultStatsDatabase : ScriptableObject
    {
        [Header("Entity Stats")]
        public SerializedDictionary<EntityType, DefaultPlayerStats> playerStats = new SerializedDictionary<EntityType, DefaultPlayerStats>();

        [field:SerializeField] public SerializedDictionary<EnemyType, DefaultEnemyStats> EnemyStats { get; private set; } = new();

        [Header("Weapon Stats")]
        public SerializedDictionary<WeaponType, DefaultWeaponStats> weaponStats = new SerializedDictionary<WeaponType, DefaultWeaponStats>();
    
        public DefaultPlayerStats GetPlayerStats(EntityType playerType = EntityType.Player)
        {
            if (playerStats.TryGetValue(playerType, out var stats))
                return stats;
            
            Debug.LogWarning($"No player stats found for {playerType}");
            return null;
        }
    
        public DefaultEnemyStats GetEnemyStats(EnemyType enemyType)
        {
            if (EnemyStats.TryGetValue(enemyType, out DefaultEnemyStats stats))
                return stats;
            
            Debug.LogWarning($"No enemy stats found for {enemyType}");
            return null;
        }
    
        public DefaultWeaponStats GetWeaponStats(WeaponType weaponType)
        {
            if (weaponStats.TryGetValue(weaponType, out var stats))
                return stats;
            
            Debug.LogWarning($"No weapon stats found for {weaponType}");
            return null;
        }
    }
}