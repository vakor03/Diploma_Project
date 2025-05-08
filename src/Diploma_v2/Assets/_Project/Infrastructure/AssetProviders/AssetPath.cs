namespace _Project.Scripts.Infrastructure.AssetProviders
{
    public static class AssetPath
    {
        public static class Prefab
        {
            public const string PLAYER_PREFAB = "PlayerPrefab";
            public const string CAMERA_PREFAB = "CameraPrefab";
        }

        public static class Configuration
        {
            public const string PREDEFINED_SEED_CONFIGURATION = "PredefinedSeedConfiguration";
            public const string LEVEL_CONFIGURATION = "LevelConfiguration_Default";
        }
    }

    public enum Layer {
        Default = 0,
        Ground = 6,
    }
}