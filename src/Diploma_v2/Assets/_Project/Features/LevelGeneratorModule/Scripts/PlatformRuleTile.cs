// PlatformRuleTile.cs - Enhanced version

using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Features.LevelGeneratorModule {
    [CreateAssetMenu(fileName = "PlatformRuleTile", menuName = "2D/Platform Rule Tile")]
    public class PlatformRuleTile : RuleTile<PlatformRuleTile.Neighbor>
    {
        [Header("Default Platform Settings")]
        public bool useOneWayCollision = true;
        public float surfaceAngle = 0f;
        public PhysicsMaterial2D platformMaterial;
    
        public class Neighbor : RuleTile.TilingRule.Neighbor
        {
            public const int ThisAndPlatform = 3;
        }
    
        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            // This sets the visual sprite based on rules
            base.GetTileData(location, tilemap, ref tileData);
        
            // Ensure we always create colliders for this tile
            tileData.colliderType = Tile.ColliderType.Sprite;
            tileData.flags = TileFlags.LockTransform;
        }
    
        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            base.RefreshTile(position, tilemap);
        
            // If there's a GameObject at this position, ensure it has proper platform setup
            GameObject tileGO = tilemap.GetComponent<Tilemap>().GetInstantiatedObject(position);
            if (tileGO != null)
            {
                EnsurePlatformSetup(tileGO);
            }
        }
    
        public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
        {
            // This is called when a new GameObject is created for the tile
            EnsurePlatformSetup(go);
        
            return true;
        }
    
        private void EnsurePlatformSetup(GameObject go)
        {
            // // Get or add collider
            // Collider2D collider = go.GetComponent<Collider2D>();
            // if (collider == null)
            // {
            //     collider = go.AddComponent<BoxCollider2D>();
            // }
            //
            // // Apply physics material
            // if (platformMaterial != null)
            // {
            //     collider.sharedMaterial = platformMaterial;
            // }
            //
            // // Add platform effector for one-way platforms
            // if (useOneWayCollision)
            // {
            //     PlatformEffector2D effector = go.GetComponent<PlatformEffector2D>();
            //     if (effector == null)
            //     {
            //         effector = go.AddComponent<PlatformEffector2D>();
            //     }
            //     
            //     effector.rotationalOffset = surfaceAngle;
            //     effector.useOneWay = true;
            //     collider.usedByEffector = true;
            // }
        }
    }
}