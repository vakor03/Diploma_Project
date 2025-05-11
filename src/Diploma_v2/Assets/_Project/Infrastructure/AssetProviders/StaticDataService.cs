using _Project.Features.Enemy;
using _Project.Features.ExperienceModule;
using _Project.Features.LevelGeneratorModule;
using _Project.Features.MapGeneration.PNGExproter;
using _Project.Features.PlayerModule;
using _Project.Features.SeedModule;
using _Project.Features.UpgradesModule.API;
using Unity.Cinemachine;
using UnityEditor.VersionControl;
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
        private TagVisualizationConfig _tagVisualizationConfig;
        private Enemy _enemyPrefab;
        private XPOrbConfiguration _xpOrbConfiguration;
        private XPLevelConfiguration _xpLevelConfiguration;
        private UpgradeDatabase _upgradeDatabase;

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

        public TagVisualizationConfig GetTagVisualizationConfig() =>
            _tagVisualizationConfig;

        public Enemy GetEnemyPrefab() =>
            _enemyPrefab;

        public XPOrbConfiguration GetXPOrbConfiguration() =>
            _xpOrbConfiguration;

        public XPLevelConfiguration GetXPLevelConfiguration() =>
            _xpLevelConfiguration;

        public void Initialize()
        {
            _playerPrefab = _assetProvider.Load<Player>(AssetPath.Prefab.PLAYER_PREFAB);
            _predefinedSeedConfiguration =
                _assetProvider.Load<PredefinedSeedConfiguration>(AssetPath.Configuration.PREDEFINED_SEED_CONFIGURATION);
            _levelConfiguration =
                _assetProvider.Load<LevelConfiguration>(AssetPath.Configuration.LEVEL_CONFIGURATION);
            _cameraPrefab =
                _assetProvider.Load<CinemachineVirtualCameraBase>(AssetPath.Prefab.CAMERA_PREFAB);
            _tagVisualizationConfig =
                _assetProvider.Load<TagVisualizationConfig>(AssetPath.Configuration.TAG_VISUALIZATION_CONFIG);
            _enemyPrefab =
                _assetProvider.Load<Enemy>(AssetPath.Prefab.ENEMY_PREFAB);
            _xpOrbConfiguration =
                _assetProvider.Load<XPOrbConfiguration>(AssetPath.Configuration.XP_ORB_CONFIGURATION);
            _xpLevelConfiguration =
                _assetProvider.Load<XPLevelConfiguration>(AssetPath.Configuration.XP_LEVEL_CONFIGURATION);
        }
    }
}