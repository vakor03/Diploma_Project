using _Project.Features.ExperienceModule;
using _Project.Infrastructure.MVP.Core;
using JetBrains.Annotations;

namespace _Project.Features.UIModule.PlayerLevelUI {
    [PublicAPI]
    public class PlayerXPPresenter : PresenterBehaviour<PlayerXPViewBase> {
        private readonly ExperienceModel _experienceModel;

        public PlayerXPPresenter(ExperienceModel experienceModel) =>
            _experienceModel = experienceModel;

        public override void OnViewSet() {
            _experienceModel.OnCurrentXPChanged += UpdateXPPercent;
            _experienceModel.OnMaxXPChanged += UpdateXPPercent;
            _experienceModel.OnLevelChanged += UpdateLevel;

            UpdateXPPercent();
            UpdateLevel();
        }

        public override void OnDisposed() {
            _experienceModel.OnCurrentXPChanged -= UpdateXPPercent;
            _experienceModel.OnMaxXPChanged -= UpdateXPPercent;
            _experienceModel.OnLevelChanged -= UpdateLevel;
        }

        private void UpdateLevel() =>
            View.SetLevel(_experienceModel.CurrentLevel);

        private void UpdateXPPercent() {
            float percent = _experienceModel.XPPercentage;
            View.SetXPPercent(percent);
        }

        private void UpdateLevel(int _) =>
            UpdateLevel();

        private void UpdateXPPercent(int _) =>
            UpdateXPPercent();
    }
}