using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Services.QueueServices
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
