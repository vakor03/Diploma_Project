using _Project.Features.ObjectPoolModule;

namespace _Project.Features.Enemy {
    public class EnemyObjectPool : GenericObjectPool<MonoPooledEnemy, EnemyType> {
        protected EnemyObjectPool(IGenericPoolFactory<MonoPooledEnemy, EnemyType> genericPoolFactory, IObjectPoolConfiguration<EnemyType> objectPoolConfiguration) : base(genericPoolFactory, objectPoolConfiguration) { }
    }
}