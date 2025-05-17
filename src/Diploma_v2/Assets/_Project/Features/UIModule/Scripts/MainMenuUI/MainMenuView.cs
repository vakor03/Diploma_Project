using UnityEngine;
using UnityEngine.UI;

namespace _Project.Features.UIModule.MainMenuUI {
    public class MainMenuView : MainMenuViewBase {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;

        private void OnEnable() {
            _playButton.onClick.AddListener(InvokeOnPlayButtonClicked);
            _exitButton.onClick.AddListener(InvokeOnExitButtonClicked);
        }
    
        private void OnDisable() {
            _playButton.onClick.RemoveListener(InvokeOnPlayButtonClicked);
            _exitButton.onClick.RemoveListener(InvokeOnExitButtonClicked);
        }
    }
}