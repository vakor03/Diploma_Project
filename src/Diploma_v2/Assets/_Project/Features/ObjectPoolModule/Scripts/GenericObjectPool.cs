using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace _Project.Features.ObjectPoolModule {
	public interface IGenericObjectPool<TPooledObject, in TType>
		where TPooledObject : class, IPooledObject<TType> where TType : Enum {
		public TPooledObject Get(TType projectileType);
		public void Release(TPooledObject pooledObject);
	}

	public class GenericObjectPool<TPooledObject, TType> : IInitializable, IGenericObjectPool<TPooledObject, TType>
		where TPooledObject : class, IPooledObject<TType> where TType : Enum {
		private readonly IGenericPoolFactory<TPooledObject, TType> _genericPoolFactory;
		private readonly IObjectPoolConfiguration<TType> _objectPoolConfiguration;

		private Dictionary<TType, IObjectPool<TPooledObject>> _objectPool;

		protected GenericObjectPool(IGenericPoolFactory<TPooledObject, TType> genericPoolFactory,
		                            IObjectPoolConfiguration<TType> objectPoolConfiguration) {
			_genericPoolFactory = genericPoolFactory;
			_objectPoolConfiguration = objectPoolConfiguration;
		}

		public void Initialize() {
			_objectPool = new();

			GameObject parent = new() { name = _objectPoolConfiguration.PoolName };

			foreach (TType value in Enum.GetValues(typeof(TType)))
				_objectPool[value] = _genericPoolFactory.Create(value, parent.transform);
		}

		public TPooledObject Get(TType projectileType) =>
			_objectPool[projectileType].Get();

		public void Release(TPooledObject pooledObject) =>
			_objectPool[pooledObject.Type].Release(pooledObject);
	}
}