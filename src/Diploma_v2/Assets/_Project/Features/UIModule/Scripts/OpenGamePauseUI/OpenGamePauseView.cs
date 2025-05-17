using UnityEngine;
using UnityEngine.UI;

namespace _Project.Features.UIModule.OpenGamePauseUI {
    internal class OpenGamePauseView : OpenGamePauseViewBase {
        [SerializeField] private Button _button;

        private void OnEnable() =>
            _button.onClick.AddListener(InvokeOnGamePauseButtonClicked);

        private void OnDisable() =>
            _button.onClick.RemoveListener(InvokeOnGamePauseButtonClicked);
    }
}