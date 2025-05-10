using System.Collections.Generic;
using System.Linq;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Matrix;
using UnityEngine;

namespace _Project.Features.MapGeneration {
    public class HorizontalSurfaceSmoother
    {
        public void SmoothHorizontalSurfaces(Matrix<BlockType> matrix, Room room)
        {
            List<HorizontalSegment> segments = FindHorizontalSegments(matrix, room);
        
            foreach (HorizontalSegment segment in segments)
            {
                SmoothSegment(matrix, segment, room);
            }
        }
    
        private List<HorizontalSegment> FindHorizontalSegments(Matrix<BlockType> matrix, Room room)
        {
            List<HorizontalSegment> segments = new();
            HashSet<Vector2Int> visited = new();
        
            foreach (Vector2Int pos in room.Cells)
            {
                if (!visited.Contains(pos) && IsHorizontalFloor(matrix, pos))
                {
                    HorizontalSegment segment = TraceHorizontalSegment(matrix, pos, visited);
                    if (segment.Length >= 3)
                    {
                        segments.Add(segment);
                    }
                }
            }
        
            return segments;
        }
    
        private bool IsHorizontalFloor(Matrix<BlockType> matrix, Vector2Int pos)
        {
            if (matrix[pos.x, pos.y] != BlockType.EmptySpace) return false;
        
            if (pos.y == 0 || matrix[pos.x, pos.y - 1] != BlockType.Wall) return true;
        
            return false;
        }
    
        private HorizontalSegment TraceHorizontalSegment(Matrix<BlockType> matrix, Vector2Int start, HashSet<Vector2Int> visited)
        {
            List<Vector2Int> segment = new();
            int y = start.y;
        
            for (int x = start.x; x >= 0; x--)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (IsHorizontalFloor(matrix, pos) && !visited.Contains(pos))
                {
                    segment.Insert(0, pos);
                    visited.Add(pos);
                }
                else
                {
                    break;
                }
            }
        
            for (int x = start.x + 1; x < matrix.Width; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (IsHorizontalFloor(matrix, pos) && !visited.Contains(pos))
                {
                    segment.Add(pos);
                    visited.Add(pos);
                }
                else
                {
                    break;
                }
            }
        
            return new HorizontalSegment(segment, y);
        }
    
        private void SmoothSegment(Matrix<BlockType> matrix, HorizontalSegment segment, Room room)
        {
            Dictionary<int, int> heightCounts = new();
        
            foreach (Vector2Int pos in segment.Positions)
            {
                int supportHeight = GetSupportHeight(matrix, pos);
                heightCounts[supportHeight] = heightCounts.GetValueOrDefault(supportHeight, 0) + 1;
            }
        
            int targetSupport = heightCounts.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        
            HashSet<Vector2Int> newCells = new HashSet<Vector2Int>(room.Cells);
            HashSet<Vector2Int> removedCells = new();
        
            for (int i = 0; i < segment.Positions.Count; i++)
            {
                Vector2Int pos = segment.Positions[i];
                AdjustFloorToHeight(matrix, pos, targetSupport, newCells, removedCells);
            }
        
            foreach (Vector2Int removed in removedCells)
            {
                newCells.Remove(removed);
            }
        
            room.Cells = newCells.ToList();
        }
    
        private int GetSupportHeight(Matrix<BlockType> matrix, Vector2Int pos)
        {
            int height = 0;
            for (int y = pos.y - 1; y >= 0; y--)
            {
                if (matrix[pos.x, y] == BlockType.Wall)
                {
                    height++;
                }
                else
                {
                    break;
                }
            }
            return height;
        }
    
        private void AdjustFloorToHeight(Matrix<BlockType> matrix, Vector2Int pos, int targetSupport, 
                                         HashSet<Vector2Int> newCells, HashSet<Vector2Int> removedCells)
        {
            for (int y = pos.y; y > pos.y - targetSupport; y--)
            {
                if (y > 0 && y < matrix.Height)
                {
                    Vector2Int currentPos = new Vector2Int(pos.x, y);
                    matrix[pos.x, y] = BlockType.EmptySpace;
                    newCells.Add(currentPos);
                }
            }
        
            int targetY = pos.y - targetSupport;
            if (targetY >= 0 && targetY < matrix.Height)
            {
                Vector2Int floorPos = new Vector2Int(pos.x, targetY);
                matrix[pos.x, targetY] = BlockType.EmptySpace;
                newCells.Add(floorPos);
            }
        
            for (int y = targetY - 1; y >= 0 && y >= targetY - targetSupport; y--)
            {
                Vector2Int wallPos = new Vector2Int(pos.x, y);
                matrix[pos.x, y] = BlockType.Wall;
                removedCells.Add(wallPos);
            }
        
            if (targetY < pos.y)
            {
                removedCells.Add(pos);
            }
        }
    }

    public class HorizontalSegment
    {
        public List<Vector2Int> Positions { get; }
        public int Height { get; }
        public int Length => Positions.Count;
    
        public HorizontalSegment(List<Vector2Int> positions, int height)
        {
            Positions = positions;
            Height = height;
        }
    }
}