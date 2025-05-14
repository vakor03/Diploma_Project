// using System.Collections.Generic;
// using _Project.Scripts.Infrastructure.AssetProviders;
// using UnityEngine;
//
// namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
//     public class ExperienceDropService : IExperienceDropService {
//         private readonly StaticDataService _staticDataService;
//         public ExperienceDropService(StaticDataService staticDataService) =>
//             _staticDataService = staticDataService;
//
//         public void DropExperience(int experienceAmount, Vector3 position) {
//             ExperienceItemsConfiguration experienceItemsConfiguration = _staticDataService.GetExperienceItemsConfiguration();
//             List<int> items = GetMinimumItems(experienceAmount, experienceItemsConfiguration.GetExperienceValuesSortedInDescending());
//             
//             foreach (int itemValue in items) {
//                 PickupItem item = experienceItemsConfiguration.ExperienceItems[itemValue];
//                 Object.Instantiate(item, position, Quaternion.identity);
//             }
//         }
//
//         public static List<int> GetMinimumItems(int requiredExperience, List<int> ItemValues) {
//             List<int> result = new();
//
//             foreach (int itemValue in ItemValues) {
//                 while (requiredExperience >= itemValue) {
//                     requiredExperience -= itemValue;
//                     result.Add(itemValue);
//                 }
//
//                 if (requiredExperience == 0)
//                     break;
//             }
//
//             if (requiredExperience != 0)
//                 Debug.LogError("Cannot achieve the exact required experience with the available items.");
//
//             return result;
//         }
//     }
// }