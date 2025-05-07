using System;
using System.Collections.Generic;
using System.Reflection;

namespace _Project.Features.Enemy.Blackboard {
    [Serializable]
    public readonly struct BlackboardKey : IEquatable<BlackboardKey> {
        private readonly string _name;
        private readonly int _hashedKey;

        public BlackboardKey(string name) {
            _name = name;
            _hashedKey = name.ComputeFNV1aHash();
        }

        public bool Equals(BlackboardKey other) =>
            _hashedKey == other._hashedKey;

        public override bool Equals(object obj) =>
            obj is BlackboardKey other && Equals(other);

        public override int GetHashCode() =>
            _hashedKey;

        public override string ToString() =>
            _name;

        public static bool operator ==(BlackboardKey lhs, BlackboardKey rhs) =>
            lhs._hashedKey == rhs._hashedKey;

        public static bool operator !=(BlackboardKey lhs, BlackboardKey rhs) =>
            !(lhs == rhs);
    }

    [Serializable]
    public class BlackboardEntry<T> {
        public BlackboardKey Key { get; }
        public T Value { get; }
        public Type ValueType { get; }

        public BlackboardEntry(BlackboardKey key, T value) {
            Key = key;
            Value = value;
            ValueType = typeof(T);
        }

        public override bool Equals(object obj) =>
            obj is BlackboardEntry<T> other && other.Key == Key;

        public override int GetHashCode() =>
            Key.GetHashCode();
    }

    [Serializable]
    public class Blackboard {
        private Dictionary<string, BlackboardKey> _keys;
        private Dictionary<BlackboardKey, object> _entries = new();

        public bool TryGetValue<T>(BlackboardKey key, out T value) {
            if (_entries.TryGetValue(key, out object entry) && entry is BlackboardEntry<T> castedEntry) {
                value = castedEntry.Value;
                return true;
            }

            value = default;
            return false;
        }

        public void SetValue<T>(BlackboardKey key, T value) =>
            _entries[key] = new BlackboardEntry<T>(key, value);

        public void Debug() {
            foreach (KeyValuePair<BlackboardKey, object> entry in _entries) {
                Type entryType = entry.Value.GetType();

                if (entryType.IsGenericType && entryType.GetGenericTypeDefinition() == typeof(BlackboardEntry<>)) {
                    PropertyInfo valueProperty = entryType.GetProperty("Value");
                    if (valueProperty == null)
                        continue;
                    
                    object value = valueProperty.GetValue(entry.Value);
                    UnityEngine.Debug.LogError($"[Blackboard.Debug] Key: {entry.Key} Value: {value}");
                }
            }
        }

        public BlackboardKey GetOrRegisterKey(string name) {
            if (_keys.TryGetValue(name, out BlackboardKey registerKey))
                return registerKey;

            BlackboardKey key = new(name);
            _keys.Add(name, key);
            return key;
        }

        public bool ContainsKey(BlackboardKey key) =>
            _entries.ContainsKey(key);

        public void Remove(BlackboardKey key) =>
            _entries.Remove(key);
    }
}