using _Project.Scripts.Core.Windows;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Bootstraps {
    public class MainMenuBootstrapper : MonoBehaviour {
        private IWindowManager _windowManager;
        
        [Inject]
        private void InjectDependencies(IWindowManager windowManager) =>
            _windowManager = windowManager;

        private void Start() =>
            _windowManager.OpenWindow<MainMenuWindow>();
    }
}