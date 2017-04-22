using System;
using System.ComponentModel;

namespace Template10.Common
{
    public delegate void TypedEventHandler<T>(object sender, T e);

    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }

    public class CancelEventArgs<T> : CancelEventArgs
    {
        public CancelEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }

    public class DeferredEventArgs : EventArgs
    {
        DeferralManager Manager;
        public DeferredEventArgs(DeferralManager manager)
        {
            Manager = manager;
        }

        public Deferral GetDeferral() => Manager.GetDeferral();
    }
}
