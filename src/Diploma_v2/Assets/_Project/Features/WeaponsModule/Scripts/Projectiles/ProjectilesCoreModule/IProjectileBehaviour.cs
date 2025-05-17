using System;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule {
	public interface IProjectileBehaviour {
		public event Action<Vector2> OnHit;
		public event Action OnDestroy;

		public void Activate();
	}
}