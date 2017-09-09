using System;
using System.Collections.Generic;

namespace Template10.Portable.Common
{
    public class ValueWithHistory<T> : IValueWithHistory<T>
    {
        public ValueWithHistory(T initial) => Value = initial;
        public ValueWithHistory(T initial, Action<string, T, T> callback)
            : this(initial) => _callback = callback;

        T _value;
        Action<string, T, T> _callback;

        public T Value
        {
            get => _value;
            set
            {
                var old = _value;
                _value = value;
                var key = $"{DateTime.UtcNow}+{Guid.NewGuid()}";
                _callback?.Invoke(key, old, value);
                History.Add(key, value);
            }
        }

        public bool ContainsValue(T value) => History.ContainsValue(value);
        public Dictionary<string, T> History { get; } = new Dictionary<string, T>();
    }
}
