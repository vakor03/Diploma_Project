using Zenject;

namespace _Project.Infrastructure.AssetLoaderModule.Core {
    public class AssetLoaderInstaller : Installer<AssetLoaderInstaller> {
        public override void InstallBindings() {
            Container.BindInterfacesTo<AddressableAssetLoaderService>()
                .AsSingle();
        }
    }
}