using _Project.Features.PlayerSpawnerModule;
using _Project.Features.StatsModule;
using _Project.Infrastructure.MVP.Core;
using JetBrains.Annotations;

namespace _Project.Features.UIModule.PlayerHealthUI {
    [PublicAPI]
    public class PlayerHealthPresenter : PresenterBehaviour<PlayerHealthViewBase> {
        private readonly PlayerStatsModel _playerStatsModel;

        public PlayerHealthPresenter(PlayerStatsModel playerStatsModel) =>
            _playerStatsModel = playerStatsModel;

        public override void OnViewSet() =>
            _playerStatsModel.Stats.OnStatChanged += OnCurrentHealthChanged;

        public override void OnDisposed() =>
            _playerStatsModel.Stats.OnStatChanged -= OnCurrentHealthChanged;

        private void OnCurrentHealthChanged(EntityStats stat, float newValue) {
            switch (stat) {
                case EntityStats.MaxHealth:
                case EntityStats.CurrentHealth:
                    UpdateCurrentHealth();
                    break;
            }
        }

        private void UpdateCurrentHealth() {
            View.SetCurrentHealth((int)_playerStatsModel.Stats[EntityStats.CurrentHealth]);
            View.SetHPBarFill(_playerStatsModel.Stats[EntityStats.CurrentHealth] / (float)_playerStatsModel.Stats[EntityStats.MaxHealth]);
        }

        private void OnCurrentHealthChanged(int currentHealth) =>
            UpdateCurrentHealth();

        private void OnMaxHealthChanged(int maxHealth) {
            UpdateCurrentHealth();
        }
    }
}