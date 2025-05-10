using UnityEngine;

public interface IEnemyFactory
{
    GameObject CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation);
    GameObject CreateEnemy(EnemyType type, Vector3 position);
} 