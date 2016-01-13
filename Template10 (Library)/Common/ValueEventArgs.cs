using System;

namespace Template10.Common
{
    public class ValueEventArgs<T> : EventArgs
    {
        public ValueEventArgs(T value)
        {
            this.Value = value;
        }

        public T Value { get; private set; }
    }
}