using System;
using System.Linq;
using System.Threading;

namespace Template10.Common
{
    public class SafeQueue<T>
    {
        private System.Collections.Generic.Queue<T> q = new System.Collections.Generic.Queue<T>();

        public event EventHandler Enqueued;

        public int Count => q.Count;

        public bool Any => q.Count > 0;

        public T[] Items => q.ToArray();

        public bool TryEnqueue(T item, int timeout = System.Threading.Timeout.Infinite)
        {
            if (Monitor.TryEnter(q, timeout))
            {
                try
                {
                    q.Enqueue(item);
                    Enqueued?.Invoke(this, EventArgs.Empty);
                }
                finally { Monitor.Exit(q); }
                return true;
            }
            else { return false; }
        }

        public bool TryDequeue(out T value, int timeout = System.Threading.Timeout.Infinite)
        {
            if (Monitor.TryEnter(q, timeout))
            {
                try
                {
                    value = q.Dequeue();
                    return true;
                }
                catch
                {
                    value = default(T);
                    return false;
                }
                finally { Monitor.Exit(q); }
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public bool TryRemove(T item, int timeout = System.Threading.Timeout.Infinite)
        {
            if (Monitor.TryEnter(q, timeout))
            {
                try
                {
                    var items = q.ToArray();
                    if (!items.Any(x => !x.Equals(item)))
                    {
                        return false;
                    }
                    q.Clear();
                    foreach (var i in items.Where(x => !x.Equals(item)))
                    {
                        q.Enqueue(i);
                    }
                    return true;
                }
                catch { return false; }
                finally { Monitor.Exit(q); }
            }
            else
            {
                return false;
            }
        }
    }
}
