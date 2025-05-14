using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Features.ObjectPoolModule {
	public interface IObjectPoolConfiguration<in TType> where TType : Enum {
		public string PoolName { get; }
		public ObjectPoolSettings GetPoolConfiguration(TType type);
	}

	public abstract class ObjectPoolConfiguration<TType> : ScriptableObject, IObjectPoolConfiguration<TType> where TType : Enum {
		[SerializeField] private string _poolName;
		[SerializeField] private ObjectPoolSettings _defaultObjectPoolSettings;
		[SerializeField] private SerializedDictionary<TType, ObjectPoolSettings> _objectPoolSettingsMap;

		public ObjectPoolSettings GetPoolConfiguration(TType type) =>
			_objectPoolSettingsMap.ContainsKey(type)
				? _objectPoolSettingsMap[type]
				: _defaultObjectPoolSettings;
		
		public string PoolName => _poolName;
	}
}