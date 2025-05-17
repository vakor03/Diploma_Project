using _Project.Features.UIModule.Windows;
using _Project.Infrastructure.MVP.Core;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Bootstraps {
    public class MainMenuBootstrapper : MonoBehaviour {
        private IWindowService _windowService;

        [Inject] 
        private void InjectDependencies(IWindowService windowService) =>
            _windowService = windowService;
        
        private void Start() =>
            _windowService.ShowWindow<MainMenuWindow>();
    }
}