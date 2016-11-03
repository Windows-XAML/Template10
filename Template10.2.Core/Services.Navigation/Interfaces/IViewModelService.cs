using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.BCL;
using Template10.Services.Lifetime;
using Template10.Services.Serialization;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.Foundation.Collections;

namespace Template10.Services.Navigation
{
    public interface IViewModelService : IService
    {
        object ResolveViewModel(Type page);

        object ResolveViewModel(Page page);

        Task CallResumeAsync(ISuspensionAware vm);

        Task CallSuspendAsync(ISuspensionAware vm);

        Task CallNavigatingAsync(INavigatingAware vm, string parameters, NavigationModes mode);
        Task CallNavigatedFromAsync(INavigationAware navigationAware);
        Task CallNavigatingFromAsync(INavigatingAware navigatingAware);
    }
}