using _Project.Features.PlayerModule;
using UnityEngine;

namespace _Project.Features.EnemyModule.Enemies.Core.ChooseTargetStrategy {
    public class PlayerTransformChooseTargetStrategy : IChooseTargetStrategy {
        private readonly Player _player;

        public PlayerTransformChooseTargetStrategy(Player player) =>
            _player = player;

        public Transform ChooseTarget() =>
            _player.transform;
    }
}