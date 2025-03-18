using _Project.Features.PlayerModule;
using _Project.Features.SeedModule;

namespace _Project.Scripts.Infrastructure.AssetProviders
{
    public interface IStaticDataService
    {
        public Player GetPlayerPrefab();
        public PredefinedSeedConfiguration GetPredefinedSeedConfiguration();
    }
}