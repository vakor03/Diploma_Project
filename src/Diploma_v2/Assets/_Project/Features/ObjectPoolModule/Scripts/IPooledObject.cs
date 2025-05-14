namespace _Project.Features.ObjectPoolModule {
	public interface IPooledObject<TType> {
		public TType Type { get; set; }
		public void OnEnabled();
		public void OnDisabled();
		public void OnDestroyed();
	}
}