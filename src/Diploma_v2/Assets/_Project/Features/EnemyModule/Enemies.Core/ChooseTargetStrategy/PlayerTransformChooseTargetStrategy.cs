using _Project.Features.PlayerModule;
using UnityEngine;

namespace _Project.Features.EnemiesModule.Scripts.Enemies.Core {
    public class PlayerTransformChooseTargetStrategy : IChooseTargetStrategy {
        private readonly Player _player;

        public PlayerTransformChooseTargetStrategy(Player player) =>
            _player = player;

        public Transform ChooseTarget() =>
            _player.transform;
    }
}