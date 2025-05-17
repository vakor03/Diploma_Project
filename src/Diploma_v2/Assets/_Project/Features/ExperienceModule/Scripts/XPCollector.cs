using _Project.Features.ExperienceModule.Orbs;
using UnityEngine;
using Zenject;

namespace _Project.Features.ExperienceModule {
    public class XPCollector : MonoBehaviour
    {
        private IExperienceService _experienceService;
    
        [Inject]
        public void InjectDependencies(IExperienceService service) =>
            _experienceService = service;

        private void OnTriggerEnter2D(Collider2D other)
        {
            XPOrb orb = other.GetComponent<XPOrb>();
            if (orb != null)
            {
                _experienceService.AddXP(Mathf.RoundToInt(orb.XPValue));
            
                orb.MoveToTarget(transform);
            }
        }
    }
}