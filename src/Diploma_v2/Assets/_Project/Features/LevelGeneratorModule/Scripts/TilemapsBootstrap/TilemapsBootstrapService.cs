using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace _Project.Features.LevelGeneratorModule.TilemapsBootstrap {
    public class TilemapsBootstrapService : ITilemapsBootstrapService {
        private readonly TilemapsBootstrapConfiguration _configuration;
        private readonly TilemapsDataHolder _tilemapsDataHolder;
        private readonly IInstantiator _instantiator;

        public TilemapsBootstrapService(TilemapsDataHolder tilemapsDataHolder, IInstantiator instantiator,
                                        IStaticDataService staticDataService) {
            _tilemapsDataHolder = tilemapsDataHolder;
            _instantiator = instantiator;
            _configuration = staticDataService.GetLevelConfiguration().TilemapsBootstrapConfiguration;
        }

        public void BootstrapTilemaps() {
            Grid grid = InstantiateGrid();
            foreach ((TilemapType tilemapType, TilemapConfiguration tilemapConfiguration) in _configuration.Tilemaps)
                _tilemapsDataHolder.RegisterTilemap(tilemapType, InstantiateTilemap(tilemapConfiguration, grid));
        }

        private Tilemap InstantiateTilemap(TilemapConfiguration tilemapConfiguration, Grid grid) {
            Tilemap tilemap = _instantiator.InstantiatePrefabForComponent<Tilemap>(tilemapConfiguration.Prefab);
            tilemap.transform.SetParent(grid.transform);
            tilemap.GetComponent<TilemapRenderer>().sortingOrder = tilemapConfiguration.OrderInLayer;
            return tilemap;
        }

        private Grid InstantiateGrid() =>
            _instantiator.InstantiatePrefabForComponent<Grid>(_configuration.GridPrefab);
    }
}