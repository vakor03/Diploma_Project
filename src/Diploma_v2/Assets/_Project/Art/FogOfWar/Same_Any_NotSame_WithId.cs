using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMap.Rule_Tiles.Custom_Rule_Tile.Tile_Asset {
    [CreateAssetMenu(menuName = "2D/Tiles/" + nameof(Same_Any_NotSame_WithId), fileName = nameof(Same_Any_NotSame_WithId), order = 0)]
    public class Same_Any_NotSame_WithId : RuleTile<Same_Any_NotSame_WithId.Neighbor> {
        [field: SerializeField]
        public int Id { get; set; }

        public class Neighbor : RuleTile.TilingRule.Neighbor {
            public const int Same = 1;
            public const int AnyNotSame = 2;
            public const int Any = 3;
        }

        public override bool RuleMatch(int neighbor, TileBase tile) {
            switch (neighbor) {
                case Neighbor.AnyNotSame: return tile == null || tile is not Same_Any_NotSame_WithId notSame || notSame.Id != Id;
                case Neighbor.Same: return tile != null && tile is Same_Any_NotSame_WithId same && same.Id == Id;
                case Neighbor.Any: return tile != null;
            }
            return base.RuleMatch(neighbor, tile);
        }
    }
}