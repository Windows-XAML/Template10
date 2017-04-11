using Template10.Common;

namespace Template10.Common
{
    public interface IQueue<T>
    {
        #region Events
        event TypedEventHandler<T> Enqueued;
        event TypedEventHandler<T> Dequeued;
        #endregion
        void Enqueue(T item);
        T Dequeue();
    }
}
