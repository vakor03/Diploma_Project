using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Features.InputModule
{
    public class InputSystem : IInputService, IInitializable, IDisposable
    {
        private InputSystem_Actions _inputSystemActions;

        public Vector2 GetLookDirection() =>
            _inputSystemActions.Player.Look.ReadValue<Vector2>();

        public Vector2 GetMoveDirection() =>
            _inputSystemActions.Player.Move.ReadValue<Vector2>();

        public event Action OnAttackStarted;
        public event Action OnAttackCanceled;

        public void Initialize()
        {
            _inputSystemActions = new InputSystem_Actions();
            _inputSystemActions.Enable();
            _inputSystemActions.Player.Enable();

            _inputSystemActions.Player.Attack.performed += HandleOnAttackPerformed;
            _inputSystemActions.Player.Attack.canceled += HandleOnAttackCanceled;
        }

        private void HandleOnAttackCanceled(InputAction.CallbackContext obj) =>
            OnAttackCanceled?.Invoke();

        private void HandleOnAttackPerformed(InputAction.CallbackContext obj) =>
            OnAttackStarted?.Invoke();

        public void Dispose()
        {
            _inputSystemActions.Player.Attack.performed -= HandleOnAttackPerformed;
            _inputSystemActions.Player.Attack.canceled -= HandleOnAttackCanceled;
            
            _inputSystemActions.Player.Disable();
            _inputSystemActions.Disable();
            _inputSystemActions = null;
        }
    }
}