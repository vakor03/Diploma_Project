namespace _Project.Features.MapGeneration {
    public interface ICloneable<out TSelf> {
        public TSelf Clone();
    }
}