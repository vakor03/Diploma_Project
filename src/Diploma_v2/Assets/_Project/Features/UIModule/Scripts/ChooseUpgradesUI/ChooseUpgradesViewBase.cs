using _Project.Infrastructure.MVP.Core;
using UnityEngine;

namespace _Project.Features.UIModule.ChooseUpgradesUI {
    public abstract class ChooseUpgradesViewBase : ViewBehaviour {
        public abstract RectTransform ContentParent { get; }
    }
}