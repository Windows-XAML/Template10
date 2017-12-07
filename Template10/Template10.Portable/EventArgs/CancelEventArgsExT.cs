using System;

namespace Template10.Common
{
    public class CancelEventArgsEx<T> : EventArgs
    {
        public CancelEventArgsEx(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

        public bool Cancel { get; set; }
    }
}
