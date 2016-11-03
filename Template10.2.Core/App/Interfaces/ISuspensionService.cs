using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Services.Lifetime;
using Template10.Services.Navigation;
using Windows.ApplicationModel;
using Template10.BCL;

namespace Template10.App
{

    public interface ISuspensionService: ILogic
    {
        Task SuspendAsync(ISuspendingDeferral deferral);
        Task RestoreAsync();
    }

}