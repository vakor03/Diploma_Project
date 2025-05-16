using Zenject;

namespace _Project.Features.ExperienceModule {
    internal class SetInitialXPSystem : IInitializable {
        private readonly ExperienceModel _model;
        private readonly XPLevelConfiguration _levelConfig;
        public SetInitialXPSystem(XPLevelConfiguration levelConfig, ExperienceModel model) {
            _levelConfig = levelConfig;
            _model = model;
        }

        public void Initialize() {
            _model.SetMaxXP(_levelConfig.GetXPForLevel(1));
            _model.SetCurrentXP(0);
            _model.SetLevel(1);
        }

    }
}