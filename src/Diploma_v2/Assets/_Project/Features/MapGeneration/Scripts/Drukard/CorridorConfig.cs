using System;
using UnityEngine;

namespace _Project.Features.MapGeneration.Drukard {
    [Serializable]
    public class CorridorConfig {
        [Range(0f, 1f)]
        public float WanderChance = 0.2f;

        [Min(1)]
        public int CorridorWidth = 1;
    }
}