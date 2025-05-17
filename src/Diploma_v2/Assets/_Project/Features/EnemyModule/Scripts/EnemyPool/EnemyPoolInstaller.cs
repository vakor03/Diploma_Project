using _Project.Extensions.ZenjectExtensions;
using _Project.Features.ObjectPoolModule;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Features.Enemy {
    public class EnemyPoolInstaller : Installer<EnemyPoolInstaller> {
        public override void InstallBindings() {
            Container.BindInterfacesAndSelfToFromAddressables<EnemyPoolConfiguration>(AssetPath.Configuration.ENEMY_POOL_CONFIGURATION)
                .AsSingle();

            Container
                .Bind<IGenericPooledObjectFactory<MonoPooledEnemy, EnemyType>>()
                .To<EnemyPooledObjectFactory>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<GenericPoolFactory<MonoPooledEnemy, EnemyType>>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyObjectPool>()
                .AsSingle();
        }
    }
}