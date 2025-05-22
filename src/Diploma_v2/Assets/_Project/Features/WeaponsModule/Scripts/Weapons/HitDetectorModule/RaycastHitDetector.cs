using _Project.Features.DamageModule;
using Features.WeaponsModule.Scripts.Weapons.WeaponsInstances;
using Global.Helpers.Scripts;
using UnityEngine;
using System.Collections.Generic;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.HitDetectorModule 
{
    public class RaycastHitDetector : IRaycastHitDetector 
    {
        private readonly LayersConfiguration _layersConfiguration;
        private readonly int _initialBufferSize = 16;
        private RaycastHit2D[] _enemyHitResults;
        private RaycastHit2D[] _environmentHitResults;
        private readonly List<HitInfo> _validHits = new();

        public RaycastHitDetector(LayersConfiguration layersConfiguration)
        {
            _layersConfiguration = layersConfiguration;
            _enemyHitResults = new RaycastHit2D[_initialBufferSize];
            _environmentHitResults = new RaycastHit2D[_initialBufferSize];
        }

        public Hit DetectHit(Vector2 startPosition, Vector2 direction, float maxDistance, LayerMask enemyLayerMask) 
        {
            _validHits.Clear();

            PerformEnemyRaycast(startPosition, direction, maxDistance, enemyLayerMask);
            PerformEnvironmentRaycast(startPosition, direction, maxDistance);

            _validHits.Sort((a, b) => a.Hit.distance.CompareTo(b.Hit.distance));
            
            foreach (HitInfo hitInfo in _validHits)
                if (hitInfo.IsEnemy) 
                {
                    HurtBox hurtBox = hitInfo.Hit.collider.GetComponent<HurtBox>();
                    if (hurtBox != null && hurtBox.Damageable != null)
                        return new TargetHit()
                            .With(targetHit => targetHit.Point = hitInfo.Hit.point)
                            .With(targetHit => targetHit.Distance = hitInfo.Hit.distance)
                            .With(targetHit => targetHit.Damageable = hurtBox.Damageable);
                } 
                else
                    return new EnvironmentHit()
                        .With(environmentHit => environmentHit.Point = hitInfo.Hit.point)
                        .With(environmentHit => environmentHit.Distance = hitInfo.Hit.distance);

            return new NullHit()
                .With(nullHit => nullHit.Point = startPosition + direction * maxDistance)
                .With(nullHit => nullHit.Distance = maxDistance);
        }

        private void PerformEnvironmentRaycast(Vector2 startPosition, Vector2 direction, float maxDistance) {
            int environmentHitCount = Physics2D.RaycastNonAlloc(
                startPosition, 
                direction, 
                _environmentHitResults, 
                maxDistance, 
                _layersConfiguration.EnvironmentLayerMask
            );

            if (environmentHitCount > _environmentHitResults.Length) {
                int newSize = Mathf.NextPowerOfTwo(environmentHitCount);
                _environmentHitResults = new RaycastHit2D[newSize];
                environmentHitCount = Physics2D.RaycastNonAlloc(
                    startPosition, 
                    direction, 
                    _environmentHitResults, 
                    maxDistance, 
                    _layersConfiguration.EnvironmentLayerMask
                );
            }

            for (int i = 0; i < environmentHitCount; i++) 
            {
                RaycastHit2D hit = _environmentHitResults[i];
                
                if (hit.collider != null) 
                {
                    _validHits.Add(new HitInfo(hit, false));
                }
            }
        }

        private void PerformEnemyRaycast(Vector2 startPosition, Vector2 direction, float maxDistance, LayerMask enemyLayerMask) {
            int enemyHitCount = Physics2D.RaycastNonAlloc(
                startPosition, 
                direction, 
                _enemyHitResults, 
                maxDistance, 
                enemyLayerMask
            );
            
            if (enemyHitCount > _enemyHitResults.Length) 
            {
                int newSize = Mathf.NextPowerOfTwo(enemyHitCount);
                _enemyHitResults = new RaycastHit2D[newSize];
                enemyHitCount = Physics2D.RaycastNonAlloc(
                    startPosition, 
                    direction, 
                    _enemyHitResults, 
                    maxDistance, 
                    enemyLayerMask
                );
            }

            for (int i = 0; i < enemyHitCount; i++) 
            {
                RaycastHit2D hit = _enemyHitResults[i];
                
                if (hit.collider != null && hit.collider.gameObject.activeSelf)
                    _validHits.Add(new HitInfo(hit, true));
            }
        }

        private class HitInfo
        {
            public RaycastHit2D Hit;
            public bool IsEnemy;
            
            public HitInfo(RaycastHit2D hit, bool isEnemy)
            {
                Hit = hit;
                IsEnemy = isEnemy;
            }
        }
    }
}