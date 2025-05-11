using System;
using System.Linq;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.Infrastructure.MVP.Core {
    public class WindowService : IWindowService {
        private readonly ActiveWindowDataHolder _activeWindowDataHolder;
        private readonly IWindowFactory _windowFactory;
        private readonly IInstantiator _instantiator;

        public WindowService(ActiveWindowDataHolder activeWindowDataHolder, IWindowFactory windowFactory, IInstantiator instantiator) {
            _activeWindowDataHolder = activeWindowDataHolder;
            _windowFactory = windowFactory;
            _instantiator = instantiator;
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
                
                foreach (PresenterBehaviour presenterBehaviour in windowBehaviour.GetAllWindowPresenters().ToList())
                    presenterBehaviour.OnViewSet();
            }
        }
        
        public TPresenter CreatePresenterForWindow<TWindow, TPresenter>(RectTransform parent, ViewBehaviour prefab)
            where TWindow : WindowBehaviour where TPresenter : PresenterBehaviour {
            if (GetWindowStatus<TWindow>() != WindowStatus.Opened)
                throw new Exception($"Window {typeof(TWindow).Name} should be opened.");
            
            Type windowType = typeof(TWindow);
            if (!_activeWindowDataHolder.WindowGameObjectMap.TryGetValue(windowType, out WindowBehaviour windowBehaviour))
                throw new Exception($"Window {typeof(TWindow).Name} not found.");

            ViewBehaviour viewBehaviour = _instantiator.InstantiatePrefabForComponent<ViewBehaviour>(prefab, parent);
            PresenterBehaviour presenter = _windowFactory.CreatePresenterForView(windowBehaviour, viewBehaviour);
            
            presenter.OnViewSet();
            
            Debug.Assert(presenter is TPresenter, $"Presenter {presenter.GetType()} is not of type {typeof(TPresenter)}");

            return presenter as TPresenter;
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