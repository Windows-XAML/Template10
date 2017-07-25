using System;
using System.Threading.Tasks;
using Template10.StartArgs;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using static Template10.Template10StartArgs;

namespace Template10.Strategies
{
    public interface IBootStrapperStrategy
    {
        BootstrapperStates Status { get; set; }
        void HandleResuming(object sender, object e);
        void HandleSuspending(object sender, SuspendingEventArgs e);
        void OnWindowCreated(WindowCreatedEventArgs args);
        void StartOrchestrationAsync(IActivatedEventArgs e, StartKinds activate);
        Func<ITemplate10StartArgs, Task> OnStartDelegate { get; set; }
        Task<UIElement> CreateRootAsync(ITemplate10StartArgs e);
        Task<UIElement> CreateSpashAsync(SplashScreen e);
    }
}
