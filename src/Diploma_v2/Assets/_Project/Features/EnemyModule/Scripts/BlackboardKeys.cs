public static class BlackboardKeys {
    public static class Bool {
        public const string IS_PLAYER_IN_RANGE = "IsPlayerInRange";
        public const string IS_CHASING_PLAYER = "IsChasingPlayer";
        public const string IS_WAITING_FOR_PLAYER = "IsWaitingForPlayer";
        public const string IS_TURRET_ACTIVE = "IsTurretActive";
    }

    public static class Vector3 {
        public const string LAST_KNOWN_PLAYER_POSITION = "LastKnownPlayerPosition";
    }

    public static class Vector2Int {
        public const string TARGET_POINT = "TargetPoint";
    }

    public static class Int {
        public const string ALLIES_IN_GROUP = "AlliesInGroup";
    }

    public static class Float {
        public const string NO_PLAYER_IN_RANGE_TIMER = "NoPlayerInRangeTimer";
    }
}