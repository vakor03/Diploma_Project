using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Features.UIModule.PlayerLevelUI {
    internal class PlayerXPView : PlayerXPViewBase {
        [SerializeField] private Slider _xpSlider;
        [SerializeField] private TMP_Text _levelText;

        public override void SetXPPercent(float percent) =>
            _xpSlider.value = percent;

        public override void SetLevel(int level) =>
            _levelText.text = level.ToString();
    }
}