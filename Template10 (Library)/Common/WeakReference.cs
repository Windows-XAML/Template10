using System;

namespace Template10.Common
{
    public class WeakReference<TInstance, TSource, TEventArgs> where TInstance : class
    {
        public WeakReference Reference { get; protected set; }
        public Action<TInstance, TSource, TEventArgs> EventAction { get; set; }
        public Action<TInstance, WeakReference<TInstance, TSource, TEventArgs>> DetachAction { get; set; }
        public WeakReference(TInstance instance) { Reference = new WeakReference(instance); }

        public virtual void Handler(TSource source, TEventArgs eventArgs)
        {
            try
            {
                EventAction(Reference?.Target as TInstance, source, eventArgs);
            }
            catch
            {
                DetachAction?.Invoke(Reference?.Target as TInstance, this);
                DetachAction = null;
            }
        }
    }

    public class WeakReference<TInstance, TSource> where TInstance : class
    {
        public WeakReference Reference { get; protected set; }
        public Action<TInstance, TSource> EventAction { get; set; }
        public Action<TInstance, WeakReference<TInstance, TSource>> DetachAction { get; set; }
        public WeakReference(TInstance instance) { Reference = new WeakReference(instance); }

        public virtual void Handler(TSource source)
        {
            try
            {
                EventAction(Reference?.Target as TInstance, source);
            }
            catch
            {
                DetachAction?.Invoke(Reference?.Target as TInstance, this);
                DetachAction = null;
            }
        }
    }
}
