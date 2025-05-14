using _Project.Features.Enemy.BehaviourTree;
using Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using UnityEngine;

namespace _Project.Features.EnemiesModule.Scripts.Enemies.Core {
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