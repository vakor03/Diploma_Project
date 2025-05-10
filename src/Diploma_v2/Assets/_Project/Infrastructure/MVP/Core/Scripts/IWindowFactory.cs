namespace _Project.Infrastructure.MVP.Core {
    public interface IWindowFactory {
        public WindowBehaviour Create<TWindow>() where TWindow : WindowBehaviour;
    }
}