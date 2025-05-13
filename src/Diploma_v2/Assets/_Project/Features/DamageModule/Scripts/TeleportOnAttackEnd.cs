using System.Collections;
using UnityEngine;

namespace _Project.Features.DamageModule {
    public class TeleportOnAttackEnd : MonoBehaviour {
        [SerializeField] private Vector2 _targetTransform;
        [SerializeField] private SimpleEnemyAttack _simpleEnemyAttack;
        [SerializeField] private Transform _rootTransform;
        
        private void OnEnable() =>
            _simpleEnemyAttack.OnAttackEnd += HandleOnAttackEnd;

        private void OnDisable() =>
            _simpleEnemyAttack.OnAttackEnd -= HandleOnAttackEnd;

        private void HandleOnAttackEnd() {
            // _rootTransform.position += (Vector3)_targetTransform;
            StartCoroutine(TeleportNextFrameCoroutine());
        }

        private IEnumerator TeleportNextFrameCoroutine() {
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            _rootTransform.position += (Vector3)_targetTransform;
        }
    }
}