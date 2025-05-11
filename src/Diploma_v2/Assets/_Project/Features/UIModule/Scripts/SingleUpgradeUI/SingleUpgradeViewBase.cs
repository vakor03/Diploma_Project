using System;
using _Project.Infrastructure.MVP.Core;
using UnityEngine;

namespace _Project.Features.UIModule.SingleUpgradeUI {
    public abstract class SingleUpgradeViewBase : ViewBehaviour {
        public event Action OnClaimUpgradeClicked;
    
        public abstract void SetIconImage(Sprite sprite);
        public abstract void SetTitle(string title);
        public abstract void SetDescription(string description);
        public abstract void SetPanelColor(Color color);

        protected void InvokeOnClaimUpgradeClicked() =>
            OnClaimUpgradeClicked?.Invoke();
    }
}