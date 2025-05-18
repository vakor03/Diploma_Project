using _Project.Features.EnemyModule.BehaviourTrees;
using _Project.Features.EnemyModule.Enemies.Core.ChooseTargetStrategy;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.EnemyModule.Enemies.Core {
    public class EnemyBehaviourTree : MonoBehaviour {
        private BehaviourTree _behaviourTree;
        // private IMoveStrategy _moveStrategy;
        private IChooseTargetStrategyFactory _chooseTargetStrategyFactory;
        [SerializeField] private WeaponSlot _weaponSlot;
        [SerializeField] private PatrolArea _patrolArea;

        [Inject]
        private void InjectDependencies(IChooseTargetStrategyFactory chooseTargetStrategyFactory) =>
            _chooseTargetStrategyFactory = chooseTargetStrategyFactory;

        private void Awake() {
            // // _moveStrategy = GetComponent<IMoveStrategy>();
            //
            // _behaviourTree = new();
            // _behaviourTree.AddChild(new Leaf("Patrol", new EntityPatrolStrategy(_patrolArea)));
            // _behaviourTree.AddChild(new Leaf("Wait", new WaitStrategy(10)));
            // // _behaviourTree.AddChild(new Leaf(new AttackStrategy(_chooseTargetStrategyFactory.Create<PlayerTransformChooseTargetStrategy>(), _weaponSlot.Weapon as IDirectionalShootable, transform),
            //     // "Attack Player"));
        }

        private void Update() =>
            _behaviourTree.Process();
    }
}