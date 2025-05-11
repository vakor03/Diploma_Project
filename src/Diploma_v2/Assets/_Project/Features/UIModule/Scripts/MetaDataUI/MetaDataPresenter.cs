using _Project.Infrastructure.MVP.Core;
using JetBrains.Annotations;

namespace _Project.Features.UIModule.MetaDataUI {
    [PublicAPI]
    public class MetaDataPresenter : PresenterBehaviour<MetaDataViewBase> {
        private const string VERSION = "0.0.1";
        public override void OnViewSet() =>
            View.SetCurrentVersion(VERSION);
    }
}