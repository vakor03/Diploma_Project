using Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponSlot {
	public interface IGenericFactory<TInterface> {
		public TInterface Create<TImplementation>() where TImplementation : TInterface;
	}

	public class GenericFactory<TInterface> : IGenericFactory<TInterface> {
		private readonly DiContainer _container;

		public GenericFactory(DiContainer container) =>
			_container = container;

		public TInterface Create<TImplementation>() where TImplementation : TInterface =>
			_container.Instantiate<TImplementation>();
	}
}