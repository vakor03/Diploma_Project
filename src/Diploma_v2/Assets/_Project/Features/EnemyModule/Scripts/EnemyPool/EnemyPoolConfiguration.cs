using _Project.Features.ObjectPoolModule;
using UnityEngine;

namespace _Project.Features.EnemyModule.EnemyPool {
    [CreateAssetMenu(fileName = nameof(EnemyPoolConfiguration) + "_Default",
        menuName = "Configurations/EnemyModule/" + nameof(EnemyPoolConfiguration))]
    public class EnemyPoolConfiguration : ObjectPoolConfiguration<EnemyType> { }
}