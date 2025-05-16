using _Project.Features.ObjectPoolModule;
using UnityEngine;

namespace _Project.Features.Enemy {
    [CreateAssetMenu(fileName = nameof(EnemyPoolConfiguration) + "_Default",
        menuName = "Configurations/EnemyModule/" + nameof(EnemyPoolConfiguration))]
    public class EnemyPoolConfiguration : ObjectPoolConfiguration<EnemyType> { }
}