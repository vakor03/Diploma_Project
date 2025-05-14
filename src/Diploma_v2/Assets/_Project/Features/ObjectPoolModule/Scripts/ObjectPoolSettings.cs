using System;
using UnityEngine;

namespace _Project.Features.ObjectPoolModule {
	[Serializable]
	public class ObjectPoolSettings {
		public GameObject Prefab;
		public int StartCapacity;
		public int DefaultCapacity;
		public int MaxSize;
	}
}