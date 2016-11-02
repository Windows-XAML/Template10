using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Foundation.Collections;
using Windows.Storage;
using Template10.BCL;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Navigation
{
    public interface ISuspensionService: IService
    {
        Task CallOnResumingAsync(String id, Page page, int backStackDepth);
        Task CallOnSuspendingAsync(String id, Page page, int backStackDepth);
    }
}