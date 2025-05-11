namespace _Project.Infrastructure.MVP.Core {
    public interface IWindowFactory {
        public WindowBehaviour Create<TWindow>() where TWindow : WindowBehaviour;
        public PresenterBehaviour CreatePresenterForView<TWindow>(TWindow windowBehaviour, ViewBehaviour viewBehaviour) where TWindow : WindowBehaviour;
    }
}