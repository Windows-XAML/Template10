using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel;

namespace Template10.Strategies
{
    public interface ILifecycleStrategyStrategy
    {
        bool IsResuming(StartupInfo e);

        Task<bool> ResumeAsync(StartupInfo e);
        Task SuspendAsync(SuspendingEventArgs e);

        event EventHandler<HandledEventArgs<StartupInfo>> Resuming;
        event EventHandler<HandledEventArgs<SuspendingEventArgs>> Suspending;
    }
}
