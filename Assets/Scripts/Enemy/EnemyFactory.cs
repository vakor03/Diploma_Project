using UnityEngine;
using Zenject;
using _Project.Scripts.Infrastructure.AssetProviders;

public class EnemyFactory : IEnemyFactory
{
    private readonly IStaticDataService _staticDataService;

    [Inject]
    public EnemyFactory(IStaticDataService staticDataService)
    {
        _staticDataService = staticDataService;
    }

    public GameObject CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = _staticDataService.EnemyConfiguration.GetEnemyPrefab(type);
        if (prefab == null)
        {
            return null;
        }

        return Object.Instantiate(prefab, position, rotation);
    }

    public GameObject CreateEnemy(EnemyType type, Vector3 position)
    {
        return CreateEnemy(type, position, Quaternion.identity);
    }
} 