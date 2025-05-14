namespace _Project.Features.WeaponsModule.Scripts.Weapons.HitDetectorModule {
	public interface IHitVisitor {
		public void Visit(Hit hit);
		public void Visit(TargetHit hit);
		public void Visit(EnvironmentHit hit);
		public void Visit(NullHit hit);
	}
}