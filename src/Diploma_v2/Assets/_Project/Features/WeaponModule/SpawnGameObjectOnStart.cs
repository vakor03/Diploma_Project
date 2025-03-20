using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponModule
{
    public class SpawnGameObjectOnStart : MonoBehaviour
    {
        [Inject] private IInstantiator _instantiator;
        [SerializeField] private GameObject _gameObject;

        private void Start()
        {
            _instantiator.InstantiatePrefab(_gameObject, transform);
        }
    }
}