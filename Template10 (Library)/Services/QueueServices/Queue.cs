using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Services.QueueServices
{
    public class Queue<T> : IQueue<T>
    {
        public Queue<T> Instance { get; } =
            new Queue<T>();

        private Queue()
        {

        }

        private System.Collections.Generic.Queue<T> _queue =
            new System.Collections.Generic.Queue<T>();

        public event TypedEventHandler<T> Enqueued;
        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
            Enqueued?.Invoke(null, item);
        }

        public event TypedEventHandler<T> Dequeued;
        public T Dequeue()
        {
            T item = _queue.Dequeue();
            Dequeued?.Invoke(null, item);
            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
