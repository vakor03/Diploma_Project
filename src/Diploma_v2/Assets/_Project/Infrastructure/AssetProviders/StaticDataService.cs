using _Project.Features.LevelGeneratorModule;
using _Project.Features.PlayerModule;
using _Project.Features.SeedModule;
using Unity.Cinemachine;
using Zenject;

namespace _Project.Scripts.Infrastructure.AssetProviders
{
    public class StaticDataService : IInitializable, IStaticDataService
    {
        private readonly IAssetProvider _assetProvider;

        private Player _playerPrefab;
        private PredefinedSeedConfiguration _predefinedSeedConfiguration;
        private LevelConfiguration _levelConfiguration;
        private CinemachineVirtualCameraBase _cameraPrefab;

        public StaticDataService(IAssetProvider assetProvider) =>
            _assetProvider = assetProvider;

        public Player GetPlayerPrefab() =>
            _playerPrefab;

        public PredefinedSeedConfiguration GetPredefinedSeedConfiguration() =>
            _predefinedSeedConfiguration;

        public LevelConfiguration GetLevelConfiguration() =>
            _levelConfiguration;

        public CinemachineVirtualCameraBase GetCameraPrefab() =>
            _cameraPrefab;

        public void Initialize()
        {
            _playerPrefab = _assetProvider.Load<Player>(AssetPath.Prefab.PLAYER_PREFAB);
            _predefinedSeedConfiguration =
                _assetProvider.Load<PredefinedSeedConfiguration>(AssetPath.Configuration.PREDEFINED_SEED_CONFIGURATION);
            _levelConfiguration =
                _assetProvider.Load<LevelConfiguration>(AssetPath.Configuration.LEVEL_CONFIGURATION);
            _cameraPrefab =
                _assetProvider.Load<CinemachineVirtualCameraBase>(AssetPath.Prefab.CAMERA_PREFAB);
        }
    }
}