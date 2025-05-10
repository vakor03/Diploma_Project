using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "EnemyFactoryInstaller", menuName = "Installers/Enemy Factory Installer")]
public class EnemyFactoryInstaller : ScriptableObjectInstaller<EnemyFactoryInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
    }
} 