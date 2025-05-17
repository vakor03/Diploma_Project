namespace _Project.Features.ExperienceModule {
    public class ExperienceService : IExperienceService {
        private readonly ExperienceModel _model;
        private readonly XPLevelConfiguration _levelConfig;

        public ExperienceService(ExperienceModel model, XPLevelConfiguration levelConfig) {
            _model = model;
            _levelConfig = levelConfig;
        }

        public void AddXP(int amount) {
            int currentXP = _model.CurrentXP;
            int remainingXP = currentXP + amount;

            while (remainingXP >= _model.MaxXP && _model.CurrentLevel < _levelConfig.MaxLevel) {
                remainingXP -= _levelConfig.GetXPForLevel(_model.CurrentLevel + 1);
                _model.IncrementLevel();
                _model.SetMaxXP(_levelConfig.GetXPForLevel(_model.CurrentLevel + 1));
            }

            _model.SetCurrentXP(remainingXP);
        }
    }
}