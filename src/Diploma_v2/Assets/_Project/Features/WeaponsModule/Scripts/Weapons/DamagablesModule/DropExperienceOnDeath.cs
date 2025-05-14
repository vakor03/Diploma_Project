using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public class DropExperienceOnDeath : MonoBehaviour {
        [SerializeField] private EntityHealth _entityHealth;
        [SerializeField] private int _experienceAmount;

        private IExperienceDropService _experienceDropService;

        [Inject]
        public void InjectDependencies(IExperienceDropService experienceDropService) =>
            _experienceDropService = experienceDropService;

        private void OnEnable() =>
            _entityHealth.OnDeath += DropExperience;

        private void OnDisable() =>
            _entityHealth.OnDeath -= DropExperience;

        private void DropExperience() =>
            _experienceDropService.DropExperience(_experienceAmount, transform.position);
    }
}