using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMap.Rule_Tiles.Custom_Rule_Tile.Tile_Asset {
    [CreateAssetMenu(menuName = "2D/Tiles/" + nameof(Same_Any_NotSame), fileName = nameof(Same_Any_NotSame), order = 0)]
    public class Same_Any_NotSame : RuleTile<Same_Any_NotSame.Neighbor> {
        public class Neighbor : RuleTile.TilingRule.Neighbor {
            public const int Same = 1;
            public const int AnyNotSame = 2;
            public const int Any = 3;
        }

        public override bool RuleMatch(int neighbor, TileBase tile) {
            switch (neighbor) {
                case Neighbor.AnyNotSame: return tile == null || (tile is not Same_Any_NotSame && tile is not Same_Any_NotSame_WithId);
                case Neighbor.Same: return tile != null && (tile is Same_Any_NotSame || tile is Same_Any_NotSame_WithId);
                case Neighbor.Any: return tile != null;
            }
            return base.RuleMatch(neighbor, tile);
        }
    }
}