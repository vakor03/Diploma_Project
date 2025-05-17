using System;
using _Project.Features.ObjectPoolModule;
using UnityEngine;

namespace _Project.Features.Enemy {
    public class MonoPooledEnemy : MonoBehaviour, IPooledObject<EnemyType> {
        public EnemyType Type { get; set; }
        public void OnEnabled() =>
            gameObject.SetActive(true);

        public void OnDisabled() =>
            gameObject.SetActive(false);

        public void OnDestroyed() =>
            Destroy(gameObject);
    }
}