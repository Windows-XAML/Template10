using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.StartArgs;
using Windows.ApplicationModel;

namespace Template10.Strategies
{
    public interface ILifecycleStrategy
    {
        bool PreviouslySuspended { get; set; }
        bool IsResuming(ITemplate10StartArgs e);

        Task<bool> ResumeAsync(ITemplate10StartArgs e);
        Task SuspendAsync(ISuspendingEventArgs e);
        Task ResumingAsync();
    }
}
