namespace _Project.Features.WeaponsModule.Scripts.Weapons.HitDetectorModule {
	public class NullHit : Hit {
		public override void Accept(IHitVisitor visitor) {
			base.Accept(visitor);
			visitor.Visit(this);
		}
	}
}