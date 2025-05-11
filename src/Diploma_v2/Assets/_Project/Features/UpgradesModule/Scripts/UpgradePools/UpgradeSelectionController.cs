using System.Collections.Generic;
using _Project.Features.GameTimeModule;
using _Project.Features.UIModule;
using _Project.Features.UIModule.ChooseUpgradesUI;
using _Project.Features.UIModule.Windows;
using _Project.Features.UpgradesModule.API;
using _Project.Infrastructure.MVP.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Features.UpgradesModule.UpgradePools {
    public class UpgradeSelectionController : MonoBehaviour
    {
        [Inject] private IUpgradePoolService _upgradePool;
        [Inject] private UpgradesToShowModel _upgradesToShowModel;
        [Inject] private IWindowService _windowService;
        [Inject] private IGamePauseService _gamePauseService;
        // [Inject] private IPlayerStatsService _playerStats;
        [SerializeField] private float _luck;
        [SerializeField] private int _level;
        [Button]
        public void ShowUpgradeSelection()
        {
            // Get player stats
            int playerLevel = _level; // _playerStats.GetLevel();
            float luckStat = _luck; // _playerStats.GetStat(StatType.Luck);
            // float luckStat = _playerStats.GetStat(StatType.Luck);

            // Get 3 random upgrades
            List<UpgradeData> upgrades = _upgradePool.GetRandomUpgrades(playerLevel, luckStat, 3);

            // Display them
            // for (int i = 0; i < upgrades.Count; i++)
            //     Debug.Log($"Upgrade {i+1}: {upgrades[i].displayName} ({upgrades[i].rarity})");
            
            _upgradesToShowModel.SetUpgradesToShow(upgrades.ConvertAll(el=>el.upgradeId));
            _gamePauseService.StopTime();
            _windowService.ShowWindow<ChooseUpgradeWindow>();
        }

        // Optional: See what rarities would be selected
        [Button]
        public void PreviewRarities()
        {
            int playerLevel = _level;
            float luckStat = _luck;

            List<UpgradeRarity> rarities = _upgradePool.SelectRarities(playerLevel, luckStat, 3);

            Debug.Log($"Selected rarities: {string.Join(", ", rarities)}");
            // Example output: "Selected rarities: Epic, Epic, Common"
        }
    }
}