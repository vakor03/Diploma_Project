using _Project.Infrastructure.AssetLoaderModule.Core;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;
using Zenject;

namespace _Project.Infrastructure.MVP.Core {
    public class WindowFactory : IWindowFactory {
        private readonly IAssetLoaderService _assetLoader;
        private readonly IPresenterFactory _presenterFactory;
        private readonly IInstantiator _instantiator;

        public WindowFactory(IAssetLoaderService assetLoader, IInstantiator instantiator, IPresenterFactory presenterFactory) {
            _assetLoader = assetLoader;
            _instantiator = instantiator;
            _presenterFactory = presenterFactory;
        }

        public WindowBehaviour Create<TWindow>() where TWindow : WindowBehaviour {
            TWindow windowBehaviour = _instantiator.Instantiate<TWindow>();

            GameObject windowInstance = InstantiateWindowPrefab<TWindow>();

            windowBehaviour.GameObject = windowInstance;

            CreatePresentersForWindow(windowInstance, windowBehaviour);

            return windowBehaviour;
        }

        private GameObject InstantiateWindowPrefab<TWindow>() where TWindow : WindowBehaviour {
            string windowKey = typeof(TWindow).Name;

            GameObject windowPrefab = _assetLoader.LoadAsset<GameObject>(windowKey, Address.Group.WINDOW);

            if (windowPrefab == null)
                throw new System.ArgumentException($"Window prefab not found for key: {windowKey} in group: {Address.Group.WINDOW}");

            GameObject windowInstance = _instantiator.InstantiatePrefab(windowPrefab);
            return windowInstance;
        }

        private void CreatePresentersForWindow<TWindow>(GameObject windowInstance, TWindow windowBehaviour)
            where TWindow : WindowBehaviour {
            foreach (ViewBehaviour viewBehaviour in windowInstance.GetComponentsInChildren<ViewBehaviour>()) {
                PresenterBehaviour presenter = _presenterFactory.CreatePresenterForView(viewBehaviour);
                windowBehaviour.RegisterPresenter(presenter);
                presenter.SetView(viewBehaviour);
            }
        }
    }
}