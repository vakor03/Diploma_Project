namespace _Project.Features.UpgradesModule.API {
    public interface IUpgradeValidationService
    {
        public bool CanApplyUpgrade(UpgradeData upgrade, int currentLevel);
    }
}