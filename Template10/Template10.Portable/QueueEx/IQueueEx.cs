namespace Template10.Common
{
    public interface IQueueEx<T>
    {
        #region Events
        event TypedEventHandler<T> Enqueued;
        event TypedEventHandler<T> Dequeued;
        #endregion
        void Enqueue(T item);
        T Dequeue();
    }
}
