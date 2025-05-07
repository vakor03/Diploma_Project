namespace _Project.Features.MapGeneration.Matrix {
    public class Matrix<T> : ICloneable<Matrix<T>> {
        private T[,] _matrix;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public T[,] ToArray() =>
            _matrix;

        public Matrix(int width, int height, T fillValue = default) {
            Width = width;
            Height = height;

            _matrix = new T[height, width];
            FillMatrix(fillValue);
        }
        
        private void FillMatrix(T value) {
            for (int i = 0; i < _matrix.GetLength(0); i++)
            for (int j = 0; j < _matrix.GetLength(1); j++)
                _matrix[i, j] = value;
        }

        public T this[int i, int j] { get => _matrix[i, j]; set => _matrix[i, j] = value; }
        
        public Matrix<T> Clone() {
            Matrix<T> newMatrix = new Matrix<T>(_matrix.GetLength(0), _matrix.GetLength(1));
            for (int i = 0; i < _matrix.GetLength(0); i++)
            for (int j = 0; j < _matrix.GetLength(1); j++)
                newMatrix._matrix[i, j] = _matrix[i, j];
            
            return newMatrix;
        }
    }
}