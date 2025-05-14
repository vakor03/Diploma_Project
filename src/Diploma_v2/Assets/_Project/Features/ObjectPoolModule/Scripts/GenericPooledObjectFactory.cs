using System;
using UnityEngine;
using Zenject;

namespace _Project.Features.ObjectPoolModule {
	public interface IGenericPooledObjectFactory<out TPooledObject, in TType>
		where TPooledObject : IPooledObject<TType> where TType : Enum {
		public TPooledObject Create(TType type, Transform parentTransform);
	}

	public sealed class
		GenericPooledObjectFactory<TPooledObject, TType> : IGenericPooledObjectFactory<TPooledObject, TType>
		where TType : Enum where TPooledObject : IPooledObject<TType> {
		private readonly DiContainer _container;
		private readonly IObjectPoolConfiguration<TType> _projectilePoolConfiguration;

		public GenericPooledObjectFactory(DiContainer container,
		                                  IObjectPoolConfiguration<TType> projectilePoolConfiguration) {
			_container = container;
			_projectilePoolConfiguration = projectilePoolConfiguration;
		}

		public TPooledObject Create(TType type, Transform parentTransform) {
			TPooledObject instantiated = _container.InstantiatePrefabForComponent<TPooledObject>(
				_projectilePoolConfiguration
					.GetPoolConfiguration(type).Prefab,
				parentTransform);

			instantiated.Type = type;

			return instantiated;
		}
	}
}