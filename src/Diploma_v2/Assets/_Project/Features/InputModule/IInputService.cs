using System;
using UnityEngine;

namespace _Project.Features.InputModule
{
    public interface IInputService
    {
        public Vector2 GetLookDirection();
        public Vector2 GetMoveDirection();

        public event Action OnAttackStarted;
        public event Action OnAttackCanceled;
    }
}