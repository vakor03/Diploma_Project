using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Features.UIModule.SingleUpgradeUI {
    internal class SingleUpgradeView : SingleUpgradeViewBase {
        [SerializeField] private Button _claimUpgradeButton;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _panelImage;

        private void OnEnable() =>
            _claimUpgradeButton.onClick.AddListener(InvokeOnClaimUpgradeClicked);
    
        private void OnDisable() =>
            _claimUpgradeButton.onClick.RemoveListener(InvokeOnClaimUpgradeClicked);
    
        public override void SetIconImage(Sprite sprite) =>
            _iconImage.sprite = sprite;
    
        public override void SetTitle(string title) =>
            _titleText.text = title;
    
        public override void SetDescription(string description) =>
            _descriptionText.text = description;
    
        public override void SetPanelColor(Color color) =>
            _panelImage.color = color;
    }
}