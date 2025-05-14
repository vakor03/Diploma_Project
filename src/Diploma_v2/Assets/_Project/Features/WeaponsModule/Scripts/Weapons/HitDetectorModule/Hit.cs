using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.HitDetectorModule {
	public abstract class Hit {
		public Vector2 Point { get; set; }
		public float Distance { get; set; }

		public virtual void Accept(IHitVisitor visitor) =>
			visitor.Visit(this);
	}
}