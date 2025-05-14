using UnityEngine;

namespace _Project.Features.EnemiesModule.Scripts.Enemies.Core {
    public interface IChooseTargetStrategy {
        public Transform ChooseTarget();
    }
}