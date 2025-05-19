using UnityEngine;
using UnityEngine.UI;

namespace _Project.Features.UIModule.GameOverUI {
    internal class GameOverView : GameOverViewBase {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _restartButton;

        private void OnEnable() {
            _mainMenuButton.onClick.AddListener(InvokeOnMainMenuButtonClicked);
            _restartButton.onClick.AddListener(InvokeOnRestartButtonClicked);
        }
    
        private void OnDisable() {
            _mainMenuButton.onClick.RemoveListener(InvokeOnMainMenuButtonClicked);
            _restartButton.onClick.RemoveListener(InvokeOnRestartButtonClicked);
        }
    }
}