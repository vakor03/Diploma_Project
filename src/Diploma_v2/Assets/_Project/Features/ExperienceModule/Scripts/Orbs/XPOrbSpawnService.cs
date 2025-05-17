using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;

namespace _Project.Features.ExperienceModule.Orbs {
    public class XPOrbSpawnService : IXPOrbSpawnService {
        private readonly IXPOrbFactory _xpOrbFactory;
        private readonly XPOrbConfiguration _orbConfig;
        private readonly IEnumValuesProvider _enumValuesProvider;

        public XPOrbSpawnService(IXPOrbFactory xpOrbFactory, IStaticDataService staticDataService, IEnumValuesProvider enumValuesProvider) {
            _xpOrbFactory = xpOrbFactory;
            _orbConfig = staticDataService.GetXPOrbConfiguration();
            _enumValuesProvider = enumValuesProvider;
        }

        public void SpawnXPOrb(float experience, Vector3 position) {
            SpawnMinimumOrbs(experience, position);
        }

        public void SpawnMinimumOrbs(float totalXP, Vector3 center) {
            List<(XPOrbType orbType, int count, float xpPerOrb)> orbsToSpawn = CalculateMinimumOrbs(totalXP, _orbConfig);
            Debug.Assert(orbsToSpawn.Sum(el=>el.count*el.xpPerOrb) == totalXP, "Total XP does not match the sum of spawned orbs.");

            int totalOrbCount = 0;
            foreach (var (orbType, count, _) in orbsToSpawn) {
                totalOrbCount += count;
            }

            int spawnedCount = 0;
            foreach ((XPOrbType orbType, int count, float xpPerOrb) in orbsToSpawn) {
                for (int i = 0; i < count; i++) {
                    float angle = (float)spawnedCount / totalOrbCount * Mathf.PI * 2f;
                    Vector3 offset = new Vector3(
                        Mathf.Cos(angle) * _orbConfig.SpawnRadius,
                        Mathf.Sin(angle) * _orbConfig.SpawnRadius,
                        0
                    );
                    Vector3 spawnPos = center + offset;

                    // Use the calculated XP value instead of maxXP
                    _xpOrbFactory.CreateXPOrb(orbType, xpPerOrb, spawnPos);
                    spawnedCount++;
                }
            }
        }

        public List<(XPOrbType orbType, int count, float xpPerOrb)> CalculateMinimumOrbs(float totalXP, XPOrbConfiguration orbConfig) {
            List<XPOrbType> orbTypesByValue = _enumValuesProvider.GetEnumValues<XPOrbType>();

            orbTypesByValue.Sort((a, b) =>
                orbConfig.GetOrbDataByType(b).maxXP.CompareTo(orbConfig.GetOrbDataByType(a).maxXP));

            List<(XPOrbType orbType, int count, float xpPerOrb)> result = new();
            float remainingXP = totalXP;

            foreach (XPOrbType orbType in orbTypesByValue) {
                XPOrbData orbData = orbConfig.GetOrbDataByType(orbType);
                if (orbData == null)
                    continue;

                int maxOrbsOfThisType = Mathf.FloorToInt(remainingXP / orbData.maxXP);

                if (maxOrbsOfThisType > 0) {
                    result.Add((orbType, maxOrbsOfThisType, orbData.maxXP));
                    remainingXP -= maxOrbsOfThisType * orbData.maxXP;
                }

                if (remainingXP <= 0)
                    break;
            }

            if (remainingXP > 0) {
                XPOrbType bestFitOrb = XPOrbType.Small;
                float minWaste = float.MaxValue;

                foreach (XPOrbType orbType in orbTypesByValue) {
                    XPOrbData orbData = orbConfig.GetOrbDataByType(orbType);
                    if (orbData.minXP <= remainingXP && remainingXP <= orbData.maxXP) {
                        bestFitOrb = orbType;
                        minWaste = 0;
                        break;
                    }
                    else if (orbData.minXP > remainingXP) {
                        float waste = orbData.minXP - remainingXP;
                        if (waste < minWaste) {
                            minWaste = waste;
                            bestFitOrb = orbType;
                        }
                    }
                }

                XPOrbData finalOrbData = orbConfig.GetOrbDataByType(bestFitOrb);
                float finalOrbXP = Mathf.Clamp(remainingXP, finalOrbData.minXP, finalOrbData.maxXP);

                bool found = false;
                for (int i = 0; i < result.Count; i++) {
                    if (result[i].orbType == bestFitOrb && result[i].xpPerOrb == finalOrbXP) {
                        result[i] = (result[i].orbType, result[i].count + 1, result[i].xpPerOrb);
                        found = true;
                        break;
                    }
                }

                if (!found)
                    result.Add((bestFitOrb, 1, finalOrbXP));
            }

            return result;
        }
    }
}