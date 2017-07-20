using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel;

namespace Template10.Strategies
{
    public interface ILifecycleStrategy
    {
        bool IsResuming(Template10StartArgs e);

        Task<bool> ResumeAsync(Template10StartArgs e);
        Task SuspendAsync(SuspendingEventArgs e);
    }
}
