using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    public interface IBootStrapper : IBootStrapperShared
    {
        Task OnInitializeAsync(OnStartEventArgs e);
        Task<UIElement> CreateRootElementAsync(IActivatedEventArgs e);
        Task OnStartAsync(OnStartEventArgs e);

        Task OnResumingAsync(OnStartEventArgs e, out bool handled);
        Task OnSuspendingAsync(SuspendingEventArgs e);

        BootstrapperStates CurrentState { get; }
        Dictionary<string, BootstrapperStates> CurrentStateHistory { get; }
    }
}