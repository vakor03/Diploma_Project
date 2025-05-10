using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _Project.Global.Collections {
    public class PriorityList<T> : IEnumerable<T>
    {
        private readonly SortedDictionary<int, List<T>> _itemsByPriority;
        private readonly Dictionary<T, int> _itemToPriority;
    
        public PriorityList()
        {
            _itemsByPriority = new SortedDictionary<int, List<T>>(Comparer<int>.Create((a, b) => b.CompareTo(a))); // Descending order
            _itemToPriority = new Dictionary<T, int>();
        }
    
        public int Count { get; private set; }
    
        /// <summary>
        /// Adds an item with the specified priority.
        /// </summary>
        public void Add(T item, int priority)
        {
            // Remove existing item if present
            if (_itemToPriority.ContainsKey(item))
            {
                Remove(item);
            }
        
            // Add to priority dictionary
            if (!_itemsByPriority.ContainsKey(priority))
            {
                _itemsByPriority[priority] = new List<T>();
            }
        
            _itemsByPriority[priority].Add(item);
            _itemToPriority[item] = priority;
            Count++;
        }
    
        /// <summary>
        /// Removes an item from the priority list.
        /// </summary>
        public bool Remove(T item)
        {
            if (!_itemToPriority.TryGetValue(item, out int priority))
            {
                return false;
            }
        
            List<T> itemsAtPriority = _itemsByPriority[priority];
            itemsAtPriority.Remove(item);
        
            // Remove the priority list if it's empty
            if (itemsAtPriority.Count == 0)
            {
                _itemsByPriority.Remove(priority);
            }
        
            _itemToPriority.Remove(item);
            Count--;
            return true;
        }
    
        /// <summary>
        /// Updates the priority of an existing item.
        /// </summary>
        public void UpdatePriority(T item, int newPriority)
        {
            if (_itemToPriority.ContainsKey(item))
            {
                Remove(item);
            }
            Add(item, newPriority);
        }
    
        /// <summary>
        /// Gets the priority of an item.
        /// </summary>
        public int? GetPriority(T item)
        {
            if (_itemToPriority.TryGetValue(item, out int priority))
            {
                return priority;
            }
            return null;
        }
    
        /// <summary>
        /// Checks if the list contains the specified item.
        /// </summary>
        public bool Contains(T item)
        {
            return _itemToPriority.ContainsKey(item);
        }
    
        /// <summary>
        /// Clears all items from the priority list.
        /// </summary>
        public void Clear()
        {
            _itemsByPriority.Clear();
            _itemToPriority.Clear();
            Count = 0;
        }
    
        /// <summary>
        /// Returns an enumerator that iterates through the collection in descending priority order.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var kvp in _itemsByPriority)
            {
                foreach (T item in kvp.Value)
                {
                    yield return item;
                }
            }
        }
    
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    
        /// <summary>
        /// Returns all items at the specified priority level.
        /// </summary>
        public IEnumerable<T> GetItemsAtPriority(int priority)
        {
            if (_itemsByPriority.TryGetValue(priority, out List<T> items))
            {
                return items.ToList(); // Return a copy to prevent external modification
            }
            return Enumerable.Empty<T>();
        }
    
        /// <summary>
        /// Returns all unique priority levels in the list.
        /// </summary>
        public IEnumerable<int> GetPriorities()
        {
            return _itemsByPriority.Keys.ToList();
        }
    
        /// <summary>
        /// Gets the item with the highest priority (first item in enumeration).
        /// </summary>
        public T? GetHighestPriorityItem()
        {
            if (Count == 0)
            {
                return default;
            }
        
            var firstPriorityGroup = _itemsByPriority.First();
            return firstPriorityGroup.Value.FirstOrDefault();
        }
    
        /// <summary>
        /// Removes and returns the item with the highest priority.
        /// </summary>
        public T? PopHighestPriority()
        {
            T? highestItem = GetHighestPriorityItem();
            if (highestItem != null)
            {
                Remove(highestItem);
            }
            return highestItem;
        }
    }
}