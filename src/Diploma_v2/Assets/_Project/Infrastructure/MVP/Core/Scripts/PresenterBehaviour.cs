namespace _Project.Infrastructure.MVP.Core {
    public abstract class PresenterBehaviour {
        protected ViewBehaviour ViewBehaviour;
        public virtual void OnViewSet() { }
        public virtual void OnDisposed() { }

        public void SetView(ViewBehaviour viewBehaviour) =>
            ViewBehaviour = viewBehaviour;
    }

    public abstract class PresenterBehaviour<TViewBehaviour> : PresenterBehaviour where TViewBehaviour : ViewBehaviour {
        protected TViewBehaviour View => ViewBehaviour as TViewBehaviour;
    }
}