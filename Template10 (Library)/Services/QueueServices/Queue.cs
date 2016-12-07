using Template10.Common;

namespace Template10.Services.QueueServices
{
    public class Queue<T> :
        IQueue<T>
    {
        public static Queue<T> Instance { get; } =
            new Queue<T>();

        private Queue()
        {

        }

        System.Collections.Generic.Queue<T> queue =
            new System.Collections.Generic.Queue<T>();

        public event TypedEventHandler<T> Enqueued;

        public void Enqueue(T item)
        {
            queue.Enqueue(item);
            Enqueued?.Invoke(null, item);
        }

        public event TypedEventHandler<T> Dequeued;

        public T Dequeue()
        {
            T item = queue.Dequeue();
            Dequeued?.Invoke(null, item);
            return item;
        }

    }
}
