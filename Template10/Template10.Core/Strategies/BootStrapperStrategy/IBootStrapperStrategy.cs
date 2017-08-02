using System;
using System.Threading.Tasks;
using Template10.StartArgs;
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
        void StartOrchestrationAsync(IActivatedEventArgs e, StartArgs.Template10StartArgs.StartKinds activate);
        Func<ITemplate10StartArgs, Task> OnStartAsyncDelegate { get; set; }
        Task<UIElement> CreateRootAsync(ITemplate10StartArgs e);
        Task<UIElement> CreateSpashAsync(SplashScreen e);

        void HandleEnteredBackground(object sender, EnteredBackgroundEventArgs e);
        void HandleLeavingBackground(object sender, LeavingBackgroundEventArgs e);
        void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e);

        Task<bool> ShowSplashAsync(ITemplate10StartArgs e);
        bool HideSplash();
    }
}
