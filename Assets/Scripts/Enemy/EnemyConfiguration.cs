using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "Game/Enemy Configuration")]
public class EnemyConfiguration : ScriptableObject
{
    [System.Serializable]
    public class EnemyData
    {
        public EnemyType type;
        public GameObject prefab;
        public EnemyStats stats;
    }

    [SerializeField] private EnemyData[] enemyConfigurations;

    public GameObject GetEnemyPrefab(EnemyType type)
    {
        foreach (var config in enemyConfigurations)
        {
            if (config.type == type)
            {
                return config.prefab;
            }
        }
        Debug.LogError($"No prefab found for enemy type: {type}");
        return null;
    }

    public EnemyData GetEnemyData(EnemyType type)
    {
        foreach (var config in enemyConfigurations)
        {
            if (config.type == type)
            {
                return config;
            }
        }
        Debug.LogError($"No configuration found for enemy type: {type}");
        return null;
    }
} 