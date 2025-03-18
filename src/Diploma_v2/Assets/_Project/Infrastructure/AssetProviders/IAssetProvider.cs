using UnityEngine;

namespace _Project.Scripts.Infrastructure.AssetProviders
{
    public interface IAssetProvider
    {
        TAsset Load<TAsset>(string key) where TAsset : Object;
    }
}