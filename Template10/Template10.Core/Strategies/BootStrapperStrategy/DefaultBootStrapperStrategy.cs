using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Portable.Common;
using Template10.StartArgs;
using Template10.Utils;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Template10.Template10StartArgs;

namespace Template10.Strategies
{
    public class DefaultBootStrapperStrategy : IBootStrapperStrategy
    {
        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null)
            => Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(DefaultBootStrapperStrategy)}.{caller}");

        public async void HandleResuming(object sender, object e) => await Template10.Settings.SuspendResumeStrategy.ResumeAsync(null);
        public async void HandleSuspending(object sender, SuspendingEventArgs e) => await Template10.Settings.SuspendResumeStrategy.SuspendAsync(e);
        public void OnWindowCreated(WindowCreatedEventArgs args) => Services.WindowWrapper.WindowWrapperFactory.Create(args);
        public async Task<UIElement> CreateRootAsync(ITemplate10StartArgs e) => await new Frame().RegisterAsync();
        public Func<ITemplate10StartArgs, Task> OnStartDelegate { get; set; } = null;
        public Task<UIElement> CreateSpashAsync(SplashScreen e) => null;

        public async void StartOrchestrationAsync(IActivatedEventArgs e, StartKinds kind)
        {
            var args = Template10StartArgsFactory.Create(e, kind);
            await OnStartDelegate?.Invoke(args);
        }

        ValueWithHistory<BootstrapperStates> _status = new ValueWithHistory<BootstrapperStates>(BootstrapperStates.None, (date, before, after) =>
        {
            DebugWrite($"{nameof(Status)} changed from {before} to {after}");
        });
        public BootstrapperStates Status
        {
            set => _status.Value = value;
            get => _status.Value;
        }

    }
}
