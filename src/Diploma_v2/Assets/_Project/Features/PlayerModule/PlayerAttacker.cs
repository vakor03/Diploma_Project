using System;
using _Project.Features.InputModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.PlayerModule
{
    public class PlayerAttacker : MonoBehaviour
    {
        [Inject] private IInputService _inputService;

        private void OnEnable() =>
            _inputService.OnAttackStarted += OnAttackHandle;

        private void OnDisable() =>
            _inputService.OnAttackStarted -= OnAttackHandle;

        private void OnAttackHandle() =>
            Debug.LogError("Attacking");
    }
}