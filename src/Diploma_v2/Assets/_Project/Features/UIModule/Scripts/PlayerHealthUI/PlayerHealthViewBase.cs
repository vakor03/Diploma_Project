using _Project.Infrastructure.MVP.Core;

namespace _Project.Features.UIModule.PlayerHealthUI {
    public abstract class PlayerHealthViewBase : ViewBehaviour {
        public abstract void SetCurrentHealth(int currentHealth);
        public abstract void SetHPBarFill(float fillAmount);
    }
}