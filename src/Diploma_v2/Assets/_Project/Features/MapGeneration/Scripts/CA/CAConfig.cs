using System;

namespace _Project.Features.MapGeneration.CA {
    [Serializable]
    public class CAConfig {
        public int offsetFromBorders = 1;
        public float fillProbability = 0.45f;
        public int steps = 5;
        public int birthLimit = 4;
        public int deathLimit = 3;
    }
}