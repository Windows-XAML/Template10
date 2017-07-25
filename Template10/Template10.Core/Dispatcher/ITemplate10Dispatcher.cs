using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Dispatcher
{
    public interface ITemplate10Dispatcher
    {
        void Dispatch(Action action, int millisecond = 0);
        Task DispatchAsync(Func<Task> action, int millisecond = 0);
    }
}