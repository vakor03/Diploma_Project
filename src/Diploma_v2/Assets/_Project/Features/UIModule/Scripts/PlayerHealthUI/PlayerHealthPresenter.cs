using _Project.Features.PlayerModule;
using _Project.Infrastructure.MVP.Core;
using JetBrains.Annotations;

namespace _Project.Features.UIModule.PlayerHealthUI {
    [PublicAPI]
    public class PlayerHealthPresenter : PresenterBehaviour<PlayerHealthViewBase> {
        private readonly PlayerHealthModel _playerHealthModel;

        public PlayerHealthPresenter(PlayerHealthModel playerHealthModel) =>
            _playerHealthModel = playerHealthModel;

        public override void OnViewSet() {
            _playerHealthModel.OnCurrentHealthChanged += OnCurrentHealthChanged;
            _playerHealthModel.OnMaxHealthChanged += OnMaxHealthChanged;
        }

        public override void OnDisposed() {
            _playerHealthModel.OnCurrentHealthChanged -= OnCurrentHealthChanged;
            _playerHealthModel.OnMaxHealthChanged -= OnMaxHealthChanged;
        }

        private void UpdateCurrentHealth() {
            View.SetCurrentHealth(_playerHealthModel.CurrentHealth);
            View.SetHPBarFill(_playerHealthModel.CurrentHealth / (float)_playerHealthModel.MaxHealth);
        }

        private void OnCurrentHealthChanged(int currentHealth) =>
            UpdateCurrentHealth();

        private void OnMaxHealthChanged(int maxHealth) {
            UpdateCurrentHealth();
        }
    }
}