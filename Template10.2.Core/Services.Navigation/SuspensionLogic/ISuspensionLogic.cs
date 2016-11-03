using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.BCL;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Navigation
{

    public interface ISuspensionLogic : ILogic
    {
        Task CallResumeAsync(ISuspensionAware vm);
        Task CallSuspendAsync(ISuspensionAware vm);
    }

}