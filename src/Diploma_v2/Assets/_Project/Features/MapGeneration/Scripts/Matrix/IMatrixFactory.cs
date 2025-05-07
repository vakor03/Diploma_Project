namespace _Project.Features.MapGeneration.Matrix {
    public interface IMatrixFactory {
        public Matrix<T> CreateMatrix<T>(int rows, int columns, T fillValue = default);
    }
}