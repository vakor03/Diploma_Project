using _Project.Infrastructure.MVP.Core;

namespace _Project.Features.UIModule.PlayerLevelUI {
    public abstract class PlayerXPViewBase : ViewBehaviour {
        public abstract void SetXPPercent(float percent);
        public abstract void SetLevel(int level);
    }
}