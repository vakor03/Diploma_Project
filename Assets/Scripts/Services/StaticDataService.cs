using UnityEngine;
using Zenject;

public interface IStaticDataService
{
    EnemyConfiguration EnemyConfiguration { get; }
    void Load();
}

public class StaticDataService : IStaticDataService
{
    private const string EnemyConfigPath = "StaticData/EnemyConfiguration";
    
    public EnemyConfiguration EnemyConfiguration { get; private set; }

    public void Load()
    {
        EnemyConfiguration = Resources.Load<EnemyConfiguration>(EnemyConfigPath);
        if (EnemyConfiguration == null)
        {
            Debug.LogError($"Failed to load EnemyConfiguration from path: {EnemyConfigPath}");
        }
    }
} 