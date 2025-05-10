namespace _Project.Infrastructure.MVP.Core {
    public interface IWindowService {
        public WindowStatus GetWindowStatus<TWindow>() where TWindow : WindowBehaviour;

        public void ShowWindow<TWindow>() where TWindow : WindowBehaviour;
        public void HideWindow<TWindow>() where TWindow : WindowBehaviour;
        public void CloseWindow<TWindow>() where TWindow : WindowBehaviour;
    }
}