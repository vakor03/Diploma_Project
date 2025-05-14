
using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.HitDetectorModule {
	public class TargetHit : Hit {
		public IDamageable Damageable { get; set; }

		public override void Accept(IHitVisitor visitor) {
			base.Accept(visitor);
			visitor.Visit(this);
		}
	}
}