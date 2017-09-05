using System;
using System.Threading.Tasks;
using Template10.BootStrap;
using Template10.Core;
using Template10.Navigation;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Template10.Strategies
{
    public interface IBootStrapperStrategy
    {
        BootstrapperStates Status { get; set; }
        void HandleResuming(object sender, object e);
        void HandleSuspending(object sender, SuspendingEventArgs e);
        void OnWindowCreated(WindowCreatedEventArgs args);
        void StartOrchestrationAsync(IActivatedEventArgs e, Core.StartArgsEx.StartKinds activate);

        Func<IStartArgsEx, INavigationService, ISessionState, Task> OnStartAsyncDelegate { get; set; }
        Func<Task> OnInitAsyncDelegate { get; set; }
        Func<IStartArgsEx, UIElement> CreateRootElementDelegate { get; set; }

        UIElement CreateRoot(IStartArgsEx e);

        void HandleEnteredBackground(object sender, EnteredBackgroundEventArgs e);
        void HandleLeavingBackground(object sender, LeavingBackgroundEventArgs e);
        void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e);
    }
}
