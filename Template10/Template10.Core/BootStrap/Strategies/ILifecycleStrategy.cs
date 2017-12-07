using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Common;
using Windows.ApplicationModel;

namespace Template10.Strategies
{
    public interface ILifecycleStrategy
    {
        bool PreviouslySuspended { get; set; }
        bool IsResuming(IStartArgsEx e);

        Task<bool> ResumeAsync(IStartArgsEx e);
        Task SuspendAsync(ISuspendingEventArgs e);
        Task ResumingAsync();
    }
}
