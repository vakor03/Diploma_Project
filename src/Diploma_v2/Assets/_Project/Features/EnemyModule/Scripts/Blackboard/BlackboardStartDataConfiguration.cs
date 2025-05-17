using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Features.EnemyModule.Blackboard {
    [CreateAssetMenu(fileName = nameof(BlackboardStartDataConfiguration) + "_Default",
        menuName = "Configurations/EnemyModule/" + nameof(BlackboardStartDataConfiguration))]
    public class BlackboardStartDataConfiguration : ScriptableObject {
        [SerializeField] private List<BlackboardEntryData> _entries = new();

        public void SetValuesOnBlackboard(Blackboard blackboard) {
            foreach (BlackboardEntryData blackboardEntryData in _entries)
                blackboardEntryData.SetValuesOnBlackboard(blackboard);
        }
    }

    [Serializable]
    public class BlackboardEntryData : ISerializationCallbackReceiver {
        public string keyName;
        public AnyValue.ValueType valueType;
        public AnyValue value;

        public void SetValuesOnBlackboard(Blackboard blackboard) {
            BlackboardKey key = blackboard.GetOrRegisterKey(keyName);
            _setValueDispatchTable[valueType].Invoke(blackboard, key, value);
        }

        private static Dictionary<AnyValue.ValueType, Action<Blackboard, BlackboardKey, AnyValue>> _setValueDispatchTable = new() {
            { AnyValue.ValueType.Bool, (blackboard, key, anyValue) => blackboard.SetValue<bool>(key, anyValue) },
        };
        
        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() =>
            value.type = valueType;
    }

    [Serializable]
    public struct AnyValue {
        public enum ValueType {
            Int = 0,
            Float = 1,
            Bool = 2,
            String = 3,
            Vector3 = 4,
        }
        
        public ValueType type;

        public bool BoolValue;

        public static implicit operator bool(AnyValue value) =>
            value.ConvertValue<bool>();

        private T ConvertValue<T>() {
            return type switch {
                ValueType.Bool => AsBool<T>(BoolValue),
                _              => throw new NotSupportedException($"Not supported value type: {typeof(T)}")
            };
        }

        private T AsBool<T>(bool value) => typeof(T)  == typeof(bool) && value is T correctType ? correctType : default;
    }
}