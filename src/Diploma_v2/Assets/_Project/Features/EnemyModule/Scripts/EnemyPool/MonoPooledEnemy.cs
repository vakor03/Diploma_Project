using _Project.Features.ObjectPoolModule;
using UnityEngine;

namespace _Project.Features.EnemyModule.EnemyPool {
    public class MonoPooledEnemy : MonoBehaviour, IPooledObject<EnemyType> {
        public EnemyType Type { get; set; }
        public void OnEnabled() =>
            gameObject.SetActive(true);

        public void OnDisabled() =>
            gameObject.SetActive(false);

        public void OnDestroyed() {
            if (gameObject == null)
                return;

            Destroy(gameObject);
        }
    }
}