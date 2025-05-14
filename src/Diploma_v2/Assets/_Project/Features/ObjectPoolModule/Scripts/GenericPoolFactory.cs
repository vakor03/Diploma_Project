using System;
using UnityEngine;
using UnityEngine.Pool;

namespace _Project.Features.ObjectPoolModule {
	public interface IGenericPoolFactory<TPooledObject, in TType> where TPooledObject : class, IPooledObject<TType> where TType : Enum {
		public IObjectPool<TPooledObject> Create(TType type, Transform parentTransform);
	}

	public sealed class GenericPoolFactory<TPooledObject, TType> : IGenericPoolFactory<TPooledObject, TType> where TPooledObject : class, IPooledObject<TType> where TType : Enum {
		private readonly IGenericPooledObjectFactory<TPooledObject, TType> _pooledObjectFactory;
		private readonly IObjectPoolConfiguration<TType> _projectilePoolConfiguration;

		public GenericPoolFactory(IGenericPooledObjectFactory<TPooledObject, TType> pooledObjectFactory,
		                             IObjectPoolConfiguration<TType> projectilePoolConfiguration) {
			_pooledObjectFactory = pooledObjectFactory;
			_projectilePoolConfiguration = projectilePoolConfiguration;
		}

		public IObjectPool<TPooledObject> Create(TType type, Transform parentTransform) {
			ObjectPoolSettings objectPoolSettings = _projectilePoolConfiguration.GetPoolConfiguration(type);

			ObjectPool<TPooledObject> objectPool = new(
				() => _pooledObjectFactory.Create(type, parentTransform), OnGet,pooled=> OnRelease(pooled, parentTransform), OnDestroy, false,
				objectPoolSettings.DefaultCapacity, objectPoolSettings.MaxSize);

			InstantiateStartObjects(objectPoolSettings, objectPool);

			return objectPool;
		}

		private static void InstantiateStartObjects(ObjectPoolSettings objectPoolSettings, ObjectPool<TPooledObject> objectPool) {
			TPooledObject[] pooledObjects = new TPooledObject[objectPoolSettings.StartCapacity];
			for (int i = 0; i < objectPoolSettings.StartCapacity; i++)
				pooledObjects[i] = objectPool.Get();

			for (int i = 0; i < objectPoolSettings.StartCapacity; i++)
				objectPool.Release(pooledObjects[i]);
		}

		private void OnDestroy(TPooledObject pooledObject) =>
			pooledObject.OnDestroyed();

		private void OnRelease(TPooledObject pooledObject, Transform parent) {
			(pooledObject as MonoBehaviour)?.transform.SetParent(parent);
			pooledObject.OnDisabled();
		}

		private void OnGet(TPooledObject pooledObject) =>
			pooledObject.OnEnabled();
	}
}