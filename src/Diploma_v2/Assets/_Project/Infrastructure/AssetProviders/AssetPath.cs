namespace _Project.Scripts.Infrastructure.AssetProviders
{
    public static class AssetPath
    {
        public static class Prefab
        {
            public const string PLAYER_PREFAB = "PlayerPrefab";
            public const string CAMERA_PREFAB = "CameraPrefab";
            public const string ENEMY_PREFAB = "EnemyPrefab";
        }

        public static class Configuration
        {
            public const string PREDEFINED_SEED_CONFIGURATION = "PredefinedSeedConfiguration";
            public const string LEVEL_CONFIGURATION = "LevelConfiguration_Default";
            public const string TAG_VISUALIZATION_CONFIG = "TagVisualizationConfig";
            public const string XP_ORB_CONFIGURATION = "XPOrbConfiguration_Default";
            public const string XP_LEVEL_CONFIGURATION = "XPLevelConfiguration_Default";
        }
    }

    public enum Layer {
        Default = 0,
        Ground = 6,
        Platform = 7,
        Enemy = 8,
        Player = 9,
    }

    public static class Address {
        public static class Group {
            public const string WINDOW = "Window";
        }
    }
}