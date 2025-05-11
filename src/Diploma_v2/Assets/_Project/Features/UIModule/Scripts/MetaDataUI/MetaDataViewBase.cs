using _Project.Infrastructure.MVP.Core;

namespace _Project.Features.UIModule.MetaDataUI {
    public abstract class MetaDataViewBase : ViewBehaviour {
        public abstract void SetCurrentVersion(string version);
    }
}