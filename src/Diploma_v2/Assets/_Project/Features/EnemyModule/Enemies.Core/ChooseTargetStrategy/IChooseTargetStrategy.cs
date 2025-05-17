using UnityEngine;

namespace _Project.Features.EnemyModule.Enemies.Core.ChooseTargetStrategy {
    public interface IChooseTargetStrategy {
        public Transform ChooseTarget();
    }
}