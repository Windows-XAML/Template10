using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Common
{

    public class WeakReference<TInstance, TSource, TEventArgs> where TInstance : class
    {
        public WeakReference<TInstance> Reference { get; protected set; }
        public Action<TInstance, TSource, TEventArgs> EventAction { get; set; }
        public Action<TInstance, WeakReference<TInstance, TSource, TEventArgs>> DetachAction { get; set; }
        public WeakReference(TInstance instance) { Reference = new WeakReference<TInstance>(instance); }

        public virtual void Handler(TSource source, TEventArgs eventArgs)
        {
            if (Reference != null && Reference.TryGetTarget(out var instance))
            {
                EventAction?.Invoke(instance, source, eventArgs);
            }
            else
            {
                // Instance surely doesn't survive garbage collections, so passing null for it
                // Don't removed unnecessary delegate parameter for backward compatibility
                DetachAction?.Invoke(null, this);
            }
        }
    }
}
