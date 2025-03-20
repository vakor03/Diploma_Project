using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponModule
{
    public class PrototypeWeapon : MonoBehaviour, IRotatable, IShootable, IWeapon
    {
        [SerializeField] private Transform _firePivot;
        [SerializeField] private Vector2 _fireDirectionLocal;

        [Inject] private IInstantiator _instantiator;

        public void RotateInDirection(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void Shoot()
        {
            Vector2 globalFireDirection = _firePivot.TransformDirection(_fireDirectionLocal);
            Debug.DrawLine(
                _firePivot.position,
                _firePivot.position + (Vector3)globalFireDirection * 5f,
                Color.red,
                1f
            );
            // var bulletPrefab = ...;
            // var bullet = _instantiator.InstantiatePrefab(
            //     bulletPrefab,
            //     _firePivot.position,
            //     Quaternion.Euler(0f, 0f, Mathf.Atan2(globalFireDirection.y, globalFireDirection.x) * Mathf.Rad2Deg),
            //     null
            // );
        }
    }
}