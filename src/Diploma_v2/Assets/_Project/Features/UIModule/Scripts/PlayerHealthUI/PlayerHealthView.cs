using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Features.UIModule.PlayerHealthUI {
    internal class PlayerHealthView : PlayerHealthViewBase {
        [SerializeField] private Slider _healthBar;
        [SerializeField] private TMP_Text _currentHealthText;
    
        public override void SetCurrentHealth(int currentHealth) =>
            _currentHealthText.text = currentHealth.ToString();

        public override void SetHPBarFill(float fillAmount) =>
            _healthBar.value = fillAmount;
    }
}