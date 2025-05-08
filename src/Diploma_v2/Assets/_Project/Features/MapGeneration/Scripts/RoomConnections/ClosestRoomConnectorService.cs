using System;
using System.Collections.Generic;
using _Project.Features.MapGeneration.BSP;
using UnityEngine;

namespace _Project.Features.MapGeneration.RoomConnections
{
    public class ClosestRoomConnectorService : IRoomConnectorService {
        public List<(Vector2Int start, Vector2Int end)> GetConnections(List<Room> rooms) {
            int roomCount = rooms.Count;
            if (roomCount < 2)
                return new List<(Vector2Int, Vector2Int)>();

            Vector2Int[] centroids = ComputeCentroids(rooms);
            List<Edge> edges = new List<Edge>();
            for (int i = 0; i < roomCount; i++) {
                for (int j = i + 1; j < roomCount; j++) {
                    float dist = Vector2Int.Distance(centroids[i], centroids[j]);
                    edges.Add(new Edge(i, j, dist));
                }
            }

            edges.Sort((a, b) => a.Distance.CompareTo(b.Distance));

            UnionFind uf = new UnionFind(roomCount);
            List<(Vector2Int, Vector2Int)> connections = new List<(Vector2Int, Vector2Int)>();

            int need = roomCount - 1;
            foreach (Edge edge in edges) {
                if (uf.Find(edge.A) != uf.Find(edge.B)) {
                    uf.Union(edge.A, edge.B);
                    (Vector2Int start, Vector2Int end) = FindClosestCellPair(
                        rooms[edge.A].Cells,
                        rooms[edge.B].Cells);
                    connections.Add((start, end));
                    need--;
                    if (need == 0)
                        break;
                }
            }

            return connections;
        }

        private Vector2Int[] ComputeCentroids(List<Room> rooms) {
            int count = rooms.Count;
            Vector2Int[] centroids = new Vector2Int[count];
            for (int i = 0; i < count; i++) {
                long sumX = 0, sumY = 0;
                List<Vector2Int> cells = rooms[i].Cells;
                int cellCount = cells.Count;
                foreach (Vector2Int cell in cells) {
                    sumX += cell.x;
                    sumY += cell.y;
                }

                centroids[i] = new Vector2Int((int)(sumX / cellCount), (int)(sumY / cellCount));
            }

            return centroids;
        }

        private (Vector2Int, Vector2Int) FindClosestCellPair(List<Vector2Int> cellsA, List<Vector2Int> cellsB) {
            int bestDist = int.MaxValue;
            Vector2Int bestA = Vector2Int.zero;
            Vector2Int bestB = Vector2Int.zero;
            foreach (Vector2Int a in cellsA) {
                foreach (Vector2Int b in cellsB) {
                    int dist = Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
                    if (dist < bestDist) {
                        bestDist = dist;
                        bestA = a;
                        bestB = b;
                    }
                }
            }

            return (bestA, bestB);
        }

        // Simple union-find for Kruskal's MST
        private class UnionFind {
            private int[] parent;

            public UnionFind(int n) {
                parent = new int[n];
                for (int i = 0; i < n; i++)
                    parent[i] = i;
            }

            public int Find(int x) =>
                parent[x] == x ? x : (parent[x] = Find(parent[x]));

            public void Union(int a, int b) {
                int pa = Find(a), pb = Find(b);
                if (pa != pb)
                    parent[pb] = pa;
            }
        }

        private class Edge {
            public int A;
            public int B;
            public float Distance;

            public Edge(int a, int b, float d) {
                A = a;
                B = b;
                Distance = d;
            }
        }
    }
}
