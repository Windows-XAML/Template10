using System;
using System.Collections.Generic;

namespace Template10.Portable.Common
{
    public class ValueWithHistory<T> : IValueWithHistory<T>
    {
        public ValueWithHistory(T initial) => Value = initial;
        public ValueWithHistory(T initial, Action<DateTime, T, T> callback)
            : this(initial) => _callback = callback;

        T _value;
        Action<DateTime, T, T> _callback;

        public T Value
        {
            get => _value;
            set
            {
                var old = _value;
                _value = value;
                _callback?.Invoke(DateTime.UtcNow, old, value);
                History.Add(DateTime.UtcNow, value);
            }
        }

        public bool ContainsValue(T value) => History.ContainsValue(value);
        public Dictionary<DateTime, T> History { get; } = new Dictionary<DateTime, T>();
    }
}
