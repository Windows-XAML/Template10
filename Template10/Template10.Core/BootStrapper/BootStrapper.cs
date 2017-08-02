using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.Container;
using Template10.Services.LoggingService;
using Template10.Services.Messenger;
using Template10.Navigation;
using Template10.StartArgs;
using Template10.Strategies;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using static Template10.StartArgs.Template10StartArgs;
using Template10.Core;

namespace Template10
{
    public abstract partial class BootStrapper : Application, IBootStrapper
    {
        static void DebugWrite(string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
                => LoggingService.WriteLine(text, severity, caller: $"{nameof(BootStrapper)}.{caller}");

        public BootStrapper()
        {
            Current = this;
            _strategy.OnStartAsyncDelegate = OnStartAsync;
            base.Resuming += _strategy.HandleResuming;
            base.Suspending += _strategy.HandleSuspending;
            base.EnteredBackground += _strategy.HandleEnteredBackground;
            base.LeavingBackground += _strategy.HandleLeavingBackground;
            base.UnhandledException += _strategy.HandleUnhandledException;
        }

        // redirected properties

        private IBootStrapperStrategy _strategy => Settings.BootStrapperStrategy;
        public IContainerService ContainerService => Settings.ContainerService;
        public IMessengerService MessengerService => Settings.MessengerService;
        public INavigationService NavigationService => Navigation.NavigationService.Default;
        public ITemplate10Dispatcher Dispatcher => Settings.DefaultDispatcher;
        public IDictionary<string, object> SessionState => SessionStateHelper.Current;

        // net-new properties 

        public new static BootStrapper Current { get; internal set; }

        // implementation methods

        public abstract Task OnStartAsync(ITemplate10StartArgs e);
        public virtual async Task<UIElement> CreateRootElement(ITemplate10StartArgs e) => await _strategy.CreateRootAsync(e);
        public virtual async Task<UIElement> CreateSpashAsync(SplashScreen e) => await _strategy.CreateSpashAsync(e);

        // hidden events

        private new event EventHandler<object> Resuming;
        private new event SuspendingEventHandler Suspending;
        private new event UnhandledExceptionEventHandler UnhandledException;
        private new event EnteredBackgroundEventHandler EnteredBackground;
        private new event LeavingBackgroundEventHandler LeavingBackground;

        // hidden overrides

        protected override sealed void OnActivated(IActivatedEventArgs e) { DebugWrite(); _strategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) { DebugWrite(); _strategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnFileActivated(FileActivatedEventArgs e) { DebugWrite(); _strategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) { DebugWrite(); _strategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) { DebugWrite(); _strategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnSearchActivated(SearchActivatedEventArgs e) { DebugWrite(); _strategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnShareTargetActivated(ShareTargetActivatedEventArgs e) { DebugWrite(); _strategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnLaunched(LaunchActivatedEventArgs e) { DebugWrite(); _strategy.StartOrchestrationAsync(e, StartKinds.Launch); }
        protected override sealed void OnBackgroundActivated(BackgroundActivatedEventArgs e) { DebugWrite(); MessengerService.Send(new Messages.BackgroundActivatedMessage { EventArgs = e }); }
        protected override sealed void OnWindowCreated(WindowCreatedEventArgs e) { DebugWrite(); _strategy.OnWindowCreated(e); }

        // clean up the object API

        public sealed override bool Equals(object obj) => base.Equals(obj);
        public sealed override int GetHashCode() => base.GetHashCode();
        public sealed override string ToString() => base.ToString();
    }
}
