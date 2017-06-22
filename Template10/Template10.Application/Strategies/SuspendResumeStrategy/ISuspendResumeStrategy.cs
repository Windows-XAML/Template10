using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel;

namespace Template10.Strategies.SuspendResumeStrategy
{
    public interface ISuspendResumeStrategy
    {
        bool IsResuming(StartupInfo e);

        Task<bool> ResumeAsync(StartupInfo e);
        Task SuspendAsync(SuspendingEventArgs e);

        event EventHandler<HandledEventArgs<StartupInfo>> Resuming;
        event EventHandler<HandledEventArgs<SuspendingEventArgs>> Suspending;
    }
}
