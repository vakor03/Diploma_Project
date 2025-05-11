using _Project.Infrastructure.AssetLoaderModule.Core;
using UnityEngine;
using Zenject;

namespace _Project.Extensions.ZenjectExtensions {
    public static class AddressablesLoaderZenjectExtensions {
        public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindConfigurationFromAddressables<T>(this DiContainer container, string addressableKey) where T : ScriptableObject {
            T configuration = container.Resolve<IAddressablesAssetLoaderService>().LoadAsset<T>(addressableKey);

            return container.Bind<T>().FromScriptableObject(configuration);
        }
    }
}