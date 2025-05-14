using System;
using _Project.Features.Enemy.BehaviourTree;
using _Project.Features.EnemyModule;
using _Project.Features.PlayerSpawnerModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.Enemy {
    public class EnemyAI : MonoBehaviour {
        [SerializeField] private EnemyMovement _enemyMovement;
        
        private BehaviourTree.BehaviourTree _tree;
        [SerializeField]private bool _isPlayerInRange;

        [Inject] private PlayerTransformDataHolder _playerTransformDataHolder;

        private void Awake() {
            _tree = new BehaviourTree.BehaviourTree("Enemy");
            Leaf isPlayerInRange = new Leaf(new Condition(()=> _isPlayerInRange), "IsPlayerInRange");
            Leaf moveToPlayer = new Leaf(new FollowStrategy(_playerTransformDataHolder.Player,_enemyMovement), "MoveToPlayer");

            Sequence goToPlayer = new Sequence("GoToPlayer");
            goToPlayer.AddChild(isPlayerInRange);
            goToPlayer.AddChild(moveToPlayer);
            
            _tree.AddChild(goToPlayer);
        }

        private void Update() {
            _tree.Process();
        }
    }
}