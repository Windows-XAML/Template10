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
        event TypedEventHandler<T> Enqueued;
        void Enqueue(T item);

        event TypedEventHandler<T> Dequeued;
        T Dequeue();
    }
}
