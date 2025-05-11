using UnityEngine;

namespace _Project.Features.UIModule.ChooseUpgradesUI {
    internal class ChooseUpgradesView : ChooseUpgradesViewBase {
        [SerializeField] private RectTransform _contentParent;

        public override RectTransform ContentParent => _contentParent;
    }
}