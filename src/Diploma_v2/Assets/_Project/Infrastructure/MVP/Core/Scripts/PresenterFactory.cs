using System;
using Zenject;

namespace _Project.Infrastructure.MVP.Core {
    public sealed class PresenterFactory : IPresenterFactory {
        private const string VIEWBASE_ENDING = "ViewBase";
        private string PRESENTER_ENDING = "Presenter";
        private readonly IInstantiator _instantiator;
        
        public PresenterFactory(IInstantiator instantiator) =>
            _instantiator = instantiator;

        public PresenterBehaviour CreatePresenterForView(ViewBehaviour view) {
            Type baseType = view.GetType().BaseType;

            try {
                Type presenterType = GetPresenterType(baseType);
                return _instantiator.Instantiate(presenterType) as PresenterBehaviour;
            }
            catch (Exception e) {
                throw new Exception($"Failed to get presenter {baseType}.", e);
            }
        }

        private Type GetPresenterType(Type baseType) =>
            Type.GetType(baseType.ToString()
                .Replace(VIEWBASE_ENDING, PRESENTER_ENDING) + ", " + baseType.Assembly);
    }
}