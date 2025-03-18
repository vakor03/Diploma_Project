using System;
using _Project.Scripts.Infrastructure.AssetProviders;
using Random = System.Random;

namespace _Project.Features.SeedModule
{
    public interface ISeedService
    {
        public Random GetRandom();
    }

    public class PredefinedSeedService : ISeedService
    {
        private readonly IStaticDataService _staticDataService;

        public PredefinedSeedService(IStaticDataService staticDataService) =>
            _staticDataService = staticDataService;

        public Random GetRandom()
        {
            PredefinedSeedConfiguration predefinedSeedConfiguration =
                _staticDataService.GetPredefinedSeedConfiguration();
            return predefinedSeedConfiguration.UseRandomSeed
                ? new Random()
                : new Random(predefinedSeedConfiguration.Seed);
        }
    }
}