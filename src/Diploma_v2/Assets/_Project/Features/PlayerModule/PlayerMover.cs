using System;
using _Project.Features.InputModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.PlayerModule
{
    public class PlayerMover : MonoBehaviour
    {
        [Inject] private IInputService _inputService;

        [SerializeField] private float _speed = 1f;

        private void Update()
        {
            Vector2 moveDirection = _inputService.GetMoveDirection();
            transform.Translate(moveDirection * (_speed * Time.deltaTime), Space.World);
        }
    }
}