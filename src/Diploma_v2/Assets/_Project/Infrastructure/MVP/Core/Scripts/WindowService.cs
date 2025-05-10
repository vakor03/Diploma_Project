using System;
using Object = UnityEngine.Object;

namespace _Project.Infrastructure.MVP.Core {
    public class WindowService : IWindowService {
        private readonly ActiveWindowDataHolder _activeWindowDataHolder;
        private readonly IWindowFactory _windowFactory;

        public WindowService(ActiveWindowDataHolder activeWindowDataHolder, IWindowFactory windowFactory) {
            _activeWindowDataHolder = activeWindowDataHolder;
            _windowFactory = windowFactory;
        }

        public WindowStatus GetWindowStatus<TWindow>() where TWindow : WindowBehaviour =>
            _activeWindowDataHolder.GetWindowStatus<TWindow>();

        public void ShowWindow<TWindow>() where TWindow : WindowBehaviour {
            if (GetWindowStatus<TWindow>() == WindowStatus.Opened)
                throw new Exception($"Window {typeof(TWindow).Name} already opened.");

            Type windowType = typeof(TWindow);

            if (GetWindowStatus<TWindow>() == WindowStatus.Hidden) {
                WindowBehaviour windowBehaviour = _activeWindowDataHolder.WindowGameObjectMap[windowType];
                windowBehaviour.GameObject.SetActive(true);
                _activeWindowDataHolder.WindowStatusMap[windowType] = WindowStatus.Opened;
            }
            else {
                WindowBehaviour windowBehaviour = _windowFactory.Create<TWindow>();
                _activeWindowDataHolder.WindowGameObjectMap[windowType] = windowBehaviour;
                _activeWindowDataHolder.WindowStatusMap[windowType] = WindowStatus.Opened;
                
                foreach (PresenterBehaviour presenterBehaviour in windowBehaviour.GetAllWindowPresenters())
                    presenterBehaviour.OnViewSet();
            }
        }

        public void HideWindow<TWindow>() where TWindow : WindowBehaviour {
            if (GetWindowStatus<TWindow>() != WindowStatus.Opened)
                throw new Exception($"Window {typeof(TWindow).Name} should be opened.");

            Type windowType = typeof(TWindow);
            WindowBehaviour windowBehaviour = _activeWindowDataHolder.WindowGameObjectMap[windowType];
            windowBehaviour.GameObject.SetActive(false);
            _activeWindowDataHolder.WindowStatusMap[windowType] = WindowStatus.Hidden;
        }

        public void CloseWindow<TWindow>() where TWindow : WindowBehaviour {
            if (GetWindowStatus<TWindow>() == WindowStatus.Closed)
                throw new Exception($"Window {typeof(TWindow).Name} already closed.");

            Type windowType = typeof(TWindow);

            if (_activeWindowDataHolder.WindowGameObjectMap.TryGetValue(windowType, out WindowBehaviour windowBehaviour)) {
                windowBehaviour.Dispose();
                Object.Destroy(windowBehaviour.GameObject);
                _activeWindowDataHolder.WindowGameObjectMap.Remove(windowType);
            }

            _activeWindowDataHolder.WindowStatusMap[windowType] = WindowStatus.Closed;
        }
    }
}