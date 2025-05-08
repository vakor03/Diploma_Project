using _Project.Features.LevelGeneratorModule;
using _Project.Features.PlayerModule;
using _Project.Features.SeedModule;
using Unity.Cinemachine;

namespace _Project.Scripts.Infrastructure.AssetProviders
{
    public interface IStaticDataService
    {
        public Player GetPlayerPrefab();
        public PredefinedSeedConfiguration GetPredefinedSeedConfiguration();
        public LevelConfiguration GetLevelConfiguration();
        public CinemachineVirtualCameraBase GetCameraPrefab();
    }
}