using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Features.Enemy {
    [RequireComponent(typeof(EntityHealthController))]
    public class EnemyDeathLogger : MonoBehaviour {
        [SerializeField, Required] private EntityHealthController _healthController;

        private void OnEnable() =>
            _healthController.Death += OnEnemyDeath;

        private void OnDisable() =>
            _healthController.Death -= OnEnemyDeath;

        private void OnEnemyDeath(DeathArgs deathArgs) {
            string logMessage = CreateDeathLogMessage(deathArgs);

            Debug.Log(logMessage);
        }

        private string CreateDeathLogMessage(DeathArgs deathArgs) {
            string enemyName = gameObject.name;
            string enemyType = GetComponent<Enemy>()?.GetType().Name ?? "EnemyModule";
            string killerName = deathArgs.Killer != null ? deathArgs.Killer.name : "Unknown";
            string deathType = deathArgs.IsInstantDeath ? "Instant Death" : "Normal Death";
            string timestamp = System.DateTime.Now.ToString("HH:mm:ss");

            return $"[{timestamp}] {enemyType} '{enemyName}' died. " +
                   $"Killer: {killerName}, " +
                   $"Death Type: {deathType}, " +
                   $"Position: {transform.position}";
        }
    }
}