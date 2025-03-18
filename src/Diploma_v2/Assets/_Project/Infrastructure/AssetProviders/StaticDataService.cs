using _Project.Features.PlayerModule;
using _Project.Features.SeedModule;
using Zenject;

namespace _Project.Scripts.Infrastructure.AssetProviders
{
    public class StaticDataService : IInitializable, IStaticDataService
    {
        private readonly IAssetProvider _assetProvider;

        private Player _playerPrefab;
        private PredefinedSeedConfiguration _predefinedSeedConfiguration;

        public StaticDataService(IAssetProvider assetProvider) =>
            _assetProvider = assetProvider;

        public Player GetPlayerPrefab() =>
            _playerPrefab;

        public PredefinedSeedConfiguration GetPredefinedSeedConfiguration() =>
            _predefinedSeedConfiguration;

        public void Initialize()
        {
            _playerPrefab = _assetProvider.Load<Player>(AssetPath.Prefab.PLAYER_PREFAB);
            _predefinedSeedConfiguration =
                _assetProvider.Load<PredefinedSeedConfiguration>(AssetPath.Configuration.PREDEFINED_SEED_CONFIGURATION);
        }
    }
}