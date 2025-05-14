// using _Project.Features.PlayerModule;
// using _Project.Features.StatsModule;
// using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;
// using _Project.Scripts.Core.Gameplay;
// using _Project.Scripts.Core.HUB;
// using UnityEngine;
// using Zenject;
//
// namespace _Project.Features.EnemiesModule.Scripts.Enemies.Core {
//     public class Player : MonoBehaviour, IEntityExperience {
//         private PlayerHealthModel _playerHealthModel;
//         private IPlayerExperienceService _playerExperienceService;
//         
//         [SerializeField] private MonoEntityStats _monoEntityStats;
//
//         [Inject]
//         public void InjectDependencies(PlayerHealthModel playerHealthModel,IPlayerExperienceService playerExperienceService) {
//             _playerHealthModel = playerHealthModel;
//             _playerExperienceService = playerExperienceService;
//         }
//
//         public void InitStats(EntityStats stats) {
//             _monoEntityStats.SetStats(stats);
//             _playerHealthModel.SetMaxHealth(_monoEntityStats.Stats[EntityStatsType.MaxHealth]);
//             _playerHealthModel.SetCurrentHealth(_monoEntityStats.Stats[EntityStatsType.CurrentHealth]);
//         }
//
//         private void OnEnable() =>
//             _monoEntityStats.OnStatChanged += OnStatChanged;
//
//         private void OnDisable() =>
//             _monoEntityStats.OnStatChanged -= OnStatChanged;
//
//         private void OnStatChanged(EntityStatsType statType, float value) {
//             switch (statType) {
//                 case EntityStatsType.CurrentHealth:
//                     _playerHealthModel.SetCurrentHealth(value);
//                     break;
//                 case EntityStatsType.MaxHealth:
//                     _playerHealthModel.SetMaxHealth(value);
//                     break;
//                 default:
//                     break;
//             }
//         }
//
//         public void AddExperience(int experienceAmount) =>
//             _playerExperienceService.AddExperience(experienceAmount);
//     }
// }