using System;

namespace Template10.Common
{
    public class ChangedEventArgs<TValue> : EventArgs
    {
        public ChangedEventArgs(TValue oldValue, TValue newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public TValue OldValue { get; }
        public TValue NewValue { get; }
    }
}