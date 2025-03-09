using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;
using _Project.Scripts.Core.Gameplay;
using _Project.Scripts.Core.Gameplay.Skills;
using _Project.Scripts.Core.HUB;
using Zenject;

namespace _Project.Scripts.Infrastructure.AssetProviders {
    public class StaticDataService : IInitializable {
        private readonly IAssetProvider _assetProvider;

        private AbilitiesSpriteMapConfiguration _abilitiesSpriteMapConfiguration;
        private AllHubCharacterVariantsConfiguration _allHubCharacterVariantsConfiguration;
        private SkillStrategiesConfigurationHolder _skillStrategiesConfigurationHolder;
        private ExperienceItemsConfiguration _experienceItemsConfiguration;
        private PlayerLevelsExperienceConfiguration _playerLevelsExperienceConfiguration;

        public StaticDataService(IAssetProvider assetProvider) =>
            _assetProvider = assetProvider;

        public void Initialize() {
            _allHubCharacterVariantsConfiguration = _assetProvider.Load<AllHubCharacterVariantsConfiguration>(AssetPath.ALL_HUB_CHARACTER_VARIANTS_CONFIGURATION);
            _abilitiesSpriteMapConfiguration = _assetProvider.Load<AbilitiesSpriteMapConfiguration>(AssetPath.ABILITIES_SPRITE_MAP_CONFIGURATION);
            _skillStrategiesConfigurationHolder = _assetProvider.Load<SkillStrategiesConfigurationHolder>(AssetPath.SKILL_STRATEGIES_CONFIGURATION_HOLDER);
            _experienceItemsConfiguration = _assetProvider.Load<ExperienceItemsConfiguration>(AssetPath.EXPERIENCE_ITEMS_CONFIGURATION);
            _playerLevelsExperienceConfiguration = _assetProvider.Load<PlayerLevelsExperienceConfiguration>(AssetPath.PLAYER_LEVELS_EXPERIENCE_CONFIGURATION);
        }

        public AllHubCharacterVariantsConfiguration GetAllChraracterVariantsConfiguration() =>
            _allHubCharacterVariantsConfiguration;

        public AbilitiesSpriteMapConfiguration GetAbilitiesSpriteMapConfiguration() =>
            _abilitiesSpriteMapConfiguration;

        public SkillStrategiesConfigurationHolder GetSkillStrategiesConfigurationHolder() =>
            _skillStrategiesConfigurationHolder;

        public ExperienceItemsConfiguration GetExperienceItemsConfiguration() =>
            _experienceItemsConfiguration;

        public PlayerLevelsExperienceConfiguration GetPlayerLevelsExperienceConfiguration() =>
            _playerLevelsExperienceConfiguration;
    }
}