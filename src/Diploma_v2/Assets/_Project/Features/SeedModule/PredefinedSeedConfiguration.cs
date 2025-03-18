using UnityEngine;

namespace _Project.Features.SeedModule
{
    [CreateAssetMenu(menuName = "Configurations/SeedModule/PredefinedSeedConfiguration", fileName = "PredefinedSeedConfiguration", order = 0)]
    public class PredefinedSeedConfiguration : ScriptableObject
    {
        public bool UseRandomSeed = false;
        public int Seed;
    }
}