using _Project.Features.EnemyModule.BehaviourTrees;
using _Project.Features.EnemyModule.Enemies.Core.ChooseTargetStrategy;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using UnityEngine;

namespace _Project.Features.EnemyModule.Enemies.Core {
    public class AttackStrategy : IStrategy {
        private readonly IChooseTargetStrategy _chooseTargetStrategy;
        private readonly IDirectionalShootable _shootable;
        private readonly Transform _transform;

        public AttackStrategy(IChooseTargetStrategy chooseTargetStrategy, IDirectionalShootable shootable, Transform transform) {
            _chooseTargetStrategy = chooseTargetStrategy;
            _shootable = shootable;
            _transform = transform;
        }

        public Node.Status Process() {
            var target = _chooseTargetStrategy.ChooseTarget();

            if (target == null)
                return Node.Status.Failure;
            
            _shootable.Shoot(target.position - _transform.position);

            Debug.LogError("Attacking Player");
            return Node.Status.Success;
        }
    }
}