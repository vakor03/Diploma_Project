namespace _Project.Features.MapGeneration.Matrix {
    public class MatrixFactory : IMatrixFactory {
        public Matrix<T> CreateMatrix<T>(int rows, int columns, T fillValue = default) =>
            new(rows, columns, fillValue);
    }
}