using System;
using System.Buffers;
using System.Collections.Generic;

namespace _Project.Extensions.EnumerableExtensions {
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns an enumerable that yields items from the list in random order.
        /// Uses ArrayPool to minimize allocations and support nested enumerations.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="list">The list to enumerate</param>
        /// <param name="random">Random number generator</param>
        /// <returns>An enumerable that yields items in random order</returns>
        public static IEnumerable<T> InRandomOrder<T>(this IList<T> list, System.Random random)
        {
            int count = list.Count;
            if (count == 0)
                yield break;
            
            // Rent an array from the shared pool
            int[] indices = ArrayPool<int>.Shared.Rent(count);
        
            try
            {
                // Initialize indices
                for (int i = 0; i < count; i++)
                {
                    indices[i] = i;
                }
            
                // Fisher-Yates shuffle
                for (int i = count - 1; i > 0; i--)
                {
                    int j = random.Next(i + 1);
                    // Swap indices
                    (indices[i], indices[j]) = (indices[j], indices[i]);
                }
            
                // Yield items in shuffled order
                for (int i = 0; i < count; i++)
                {
                    yield return list[indices[i]];
                }
            }
            finally
            {
                // Return the array to the pool when enumeration is complete
                ArrayPool<int>.Shared.Return(indices);
            }
        }
    
        /// <summary>
        /// More efficient version for when you need to process all items at once.
        /// Uses ArrayPool and Span to avoid allocations.
        /// </summary>
        public static void ForEachRandom<T>(this IList<T> list, System.Random random, Action<T> action)
        {
            int count = list.Count;
            if (count == 0) return;
        
            // Rent an array from the shared pool
            int[] indices = ArrayPool<int>.Shared.Rent(count);
        
            try
            {
                // Create a span over the rented array (no allocation)
                Span<int> indicesSpan = indices.AsSpan(0, count);
            
                // Initialize indices
                for (int i = 0; i < count; i++)
                {
                    indicesSpan[i] = i;
                }
            
                // Fisher-Yates shuffle
                for (int i = count - 1; i > 0; i--)
                {
                    int j = random.Next(i + 1);
                    // Swap indices
                    int temp = indicesSpan[i];
                    indicesSpan[i] = indicesSpan[j];
                    indicesSpan[j] = temp;
                }
            
                // Process items in shuffled order
                for (int i = 0; i < count; i++)
                {
                    action(list[indicesSpan[i]]);
                }
            }
            finally
            {
                // Return the array to the pool
                ArrayPool<int>.Shared.Return(indices);
            }
        }
    }
}