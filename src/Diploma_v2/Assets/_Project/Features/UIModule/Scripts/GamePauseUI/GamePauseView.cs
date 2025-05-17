using UnityEngine;
using UnityEngine.UI;

namespace _Project.Features.UIModule.GamePauseUI {
    internal class GamePauseView : GamePauseViewBase {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _backToMainMenuButton;
        [SerializeField] private Button _exitButton;
    
        private void OnEnable() {
            _resumeButton.onClick.AddListener(InvokeOnResumeButtonClicked);
            _backToMainMenuButton.onClick.AddListener(InvokeOnBackToMainMenuButtonClicked);
            _exitButton.onClick.AddListener(InvokeOnExitButtonClicked);
        }
    
        private void OnDisable() {
            _resumeButton.onClick.RemoveListener(InvokeOnResumeButtonClicked);
            _backToMainMenuButton.onClick.RemoveListener(InvokeOnBackToMainMenuButtonClicked);
            _exitButton.onClick.RemoveListener(InvokeOnExitButtonClicked);
        }
    }
}