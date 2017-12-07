using System;

namespace Template10.Common
{
    public class EventArgsEx<T> : EventArgs
    {
        public EventArgsEx(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}
