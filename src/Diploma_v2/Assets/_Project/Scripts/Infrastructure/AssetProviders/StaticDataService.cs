using Zenject;

namespace _Project.Scripts.Infrastructure.AssetProviders
{
    public class StaticDataService : IInitializable
    {
        private readonly IAssetProvider _assetProvider;

        public StaticDataService(IAssetProvider assetProvider) =>
            _assetProvider = assetProvider;

        public void Initialize()
        {
        }
    }
}