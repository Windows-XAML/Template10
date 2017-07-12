using System;
using System.Collections.Generic;

namespace Template10.Portable.Common
{
    public class ValueWithHistory<T> : IValueWithHistory<T>
    {
        public ValueWithHistory(T initial) => Value = initial;
        public ValueWithHistory(T initial, Action<DateTime, T> callback)
            : this(initial) => _callback = callback;

        T _value;
        Action<DateTime, T> _callback;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                History.Add(DateTime.UtcNow, value);
                _callback?.Invoke(DateTime.UtcNow, value);
            }
        }

        public bool ContainsValue(T value) => History.ContainsValue(value);
        public Dictionary<DateTime, T> History { get; } = new Dictionary<DateTime, T>();
    }
}
