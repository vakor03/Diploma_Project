using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Features.MapGeneration.Matrix {
    public class Matrix<T> : ICloneable<Matrix<T>> {
        private T[,] _matrix;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public T[,] ToArray() =>
            _matrix;

        public T this[int i, int j] { get => _matrix[i, j]; set => _matrix[i, j] = value; }

        public Matrix(int width, int height, T fillValue = default) {
            Width = width;
            Height = height;

            _matrix = new T[height, width];
            FillMatrix(fillValue);
        }

        public Matrix<T> Clone() {
            Matrix<T> newMatrix = new Matrix<T>(_matrix.GetLength(0), _matrix.GetLength(1));
            for (int i = 0; i < _matrix.GetLength(0); i++)
            for (int j = 0; j < _matrix.GetLength(1); j++)
                newMatrix._matrix[i, j] = _matrix[i, j];
            
            return newMatrix;
        }
        
        public IEnumerable<Vector2Int> GetAllIndices(Predicate<T> predicate) {
            for (int i = 0; i < _matrix.GetLength(0); i++)
            for (int j = 0; j < _matrix.GetLength(1); j++)
                if (predicate(_matrix[i, j]))
                    yield return new Vector2Int(i, j);
        }

        private void FillMatrix(T value) {
            for (int i = 0; i < _matrix.GetLength(0); i++)
            for (int j = 0; j < _matrix.GetLength(1); j++)
                _matrix[i, j] = value;
        }


        public void Crop(Predicate<T> predicate) {
            int minRow = int.MaxValue, maxRow = int.MinValue;
            int minCol = int.MaxValue, maxCol = int.MinValue;

            for (int row = 0; row < Height; row++) {
                for (int col = 0; col < Width; col++) {
                    if (predicate(_matrix[row, col])) {
                        if (row < minRow) minRow = row;
                        if (row > maxRow) maxRow = row;
                        if (col < minCol) minCol = col;
                        if (col > maxCol) maxCol = col;
                    }
                }
            }

            if (maxRow < minRow || maxCol < minCol) {
                _matrix = new T[0, 0];
                Width = 0;
                Height = 0;
                return;
            }

            int newHeight = maxRow - minRow + 1;
            int newWidth = maxCol - minCol + 1;
            T[,] cropped = new T[newHeight, newWidth];

            for (int row = 0; row < newHeight; row++) {
                for (int col = 0; col < newWidth; col++) {
                    cropped[row, col] = _matrix[minRow + row, minCol + col];
                }
            }

            _matrix = cropped;
            Height = newHeight;
            Width = newWidth;
        }

        public T this[Vector2Int position] {get=> _matrix[position.x, position.y]; set => _matrix[position.x, position.y] = value; }
    }
}