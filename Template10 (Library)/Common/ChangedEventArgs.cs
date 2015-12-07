using System;

namespace Template10.Common
{
    public class ChangedEventArgs<TValue> : EventArgs
    {
        private readonly TValue oldValue;
        private readonly TValue newValue;

        public ChangedEventArgs(TValue oldValue, TValue newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public TValue OldValue => this.oldValue;

        public TValue NewValue => this.newValue;
    }
}