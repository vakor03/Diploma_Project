using System;
using _Project.Features.InputModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.PlayerModule
{
    public class PlayerWeaponRotator : MonoBehaviour
    {
        [Inject] private IInputService _inputService;

        private void OnDrawGizmos()
        {
            if (_inputService == null)
                return;

            Vector2 vectorDirection = _inputService.GetLookDirection();
            Debug.DrawRay(transform.position, vectorDirection * 2f, Color.green);
        }
    }
}