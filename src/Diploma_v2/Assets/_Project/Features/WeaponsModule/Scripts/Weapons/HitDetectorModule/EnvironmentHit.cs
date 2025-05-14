namespace _Project.Features.WeaponsModule.Scripts.Weapons.HitDetectorModule {
	public class EnvironmentHit : Hit {
		public override void Accept(IHitVisitor visitor) {
			base.Accept(visitor);
			visitor.Visit(this);
		}
	}
}