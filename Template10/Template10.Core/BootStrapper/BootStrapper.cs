using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Services.Container;
using Template10.Services.Logging;
using Template10.Navigation;
using Template10.Core;
using Template10.Strategies;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using static Template10.Core.StartArgsEx;
using Template10.Services.Messenger;
using Template10.Services.Serialization;
using Template10.Services.Gesture;

namespace Template10
{
    public abstract partial class BootStrapper : ILoggable
    {
        ILoggingService ILoggable.LoggingService => Central.LoggingService;
        void LogThis(string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
            => (this as ILoggable).LogThis(text, severity, caller: $"{caller}");
        void ILoggable.LogThis(string text, Severities severity, string caller)
            => (this as ILoggable).LoggingService.WriteLine(text, severity, caller: $"{caller}");
    }

    public abstract partial class BootStrapper : Application, IBootStrapper
    {
        public BootStrapper(IBootStrapperStrategy strategy = null)
        {
            Current = this;

            RegisterDependencyObjects(strategy);

            LogThis($"After {nameof(RegisterDependencyObjects)}. This is the first Template 10 trace.");

            BootStrapperStrategy.OnStartAsyncDelegate = OnStartAsync;
            BootStrapperStrategy.OnInitAsyncDelegate = OnInitializeAsync;
            BootStrapperStrategy.CreateSpashDelegate = CreateSpash;
            BootStrapperStrategy.CreateRootElementDelegate = CreateRootElement;

            base.Resuming += BootStrapperStrategy.HandleResuming;
            base.Suspending += BootStrapperStrategy.HandleSuspending;
            base.EnteredBackground += BootStrapperStrategy.HandleEnteredBackground;
            base.LeavingBackground += BootStrapperStrategy.HandleLeavingBackground;
            base.UnhandledException += BootStrapperStrategy.HandleUnhandledException;
        }

        void RegisterDependencyObjects(IBootStrapperStrategy bootStrapperStrategy)
        {
            // services
            if (Services.Container.ContainerService.Default == null)
            {
                Services.Container.ContainerService.Default = new UnityContainerService();
            }
            ContainerService.Register<ISessionState, SessionState>();
            ContainerService.Register<ILoggingService, LoggingService>();
            // TODO: remove this ContainerService.Register<IMessengerService, MvvmLightMessengerService>();
            ContainerService.Register<ISerializationService, JsonSerializationService>();
            ContainerService.Register<IBackButtonService, BackButtonService>();
            ContainerService.Register<IKeyboardService, KeyboardService>();

            // strategies
            if (bootStrapperStrategy == null)
            {
                ContainerService.Register<IBootStrapperStrategy, DefaultBootStrapperStrategy>();
            }
            else
            {
                ContainerService.Register<IBootStrapperStrategy>(bootStrapperStrategy);
            }
            ContainerService.Register<ILifecycleStrategy, DefaultLifecycleStrategy>();
            ContainerService.Register<IStateStrategy, DefaultStateStrategy>();
            ContainerService.Register<ITitleBarStrategy, DefaultTitleBarStrategy>();
            ContainerService.Register<IExtendedSessionStrategy, DefaultExtendedSessionStrategy>();
            ContainerService.Register<IViewModelActionStrategy, DefaultViewModelActionStrategy>();
            ContainerService.Register<IViewModelResolutionStrategy, DefaultViewModelResolutionStrategy>();

            // delay some things until after first window is created
            Central.MessengerService.Subscribe<Messages.WindowCreatedMessage>(this, e =>
            {
                ContainerService.Resolve<IBackButtonService>().Setup(); // TODO: unsubscribe
            });
        }

        // redirected properties

        private IBootStrapperStrategy BootStrapperStrategy => ContainerService.Resolve<IBootStrapperStrategy>();
        public IContainerService ContainerService => Services.Container.ContainerService.Default;
        public IMessengerService MessengerService => Central.MessengerService;
        public INavigationService NavigationService => Navigation.NavigationService.Default;
        public IDispatcherEx Dispatcher => WindowEx.GetDefault().Dispatcher;
        public ISessionState SessionState => Central.SessionState;

        // net-new properties 

        public new static BootStrapper Current { get; internal set; }

        // implementation methods

        public virtual Task OnInitializeAsync() => Task.CompletedTask;
        public abstract Task OnStartAsync(IStartArgsEx e);
        public virtual UIElement CreateRootElement(IStartArgsEx e) => null;
        public virtual UIElement CreateSpash(SplashScreen e) => null;

        // clean up the Application events

#pragma warning disable CS0067
        private new event EventHandler<object> Resuming;
        private new event SuspendingEventHandler Suspending;
        private new event UnhandledExceptionEventHandler UnhandledException;
        private new event EnteredBackgroundEventHandler EnteredBackground;
        private new event LeavingBackgroundEventHandler LeavingBackground;
#pragma warning restore CS0067

        // clean up the Application overrides

        protected override sealed void OnActivated(IActivatedEventArgs e) { LogThis(); BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) { LogThis(); BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnFileActivated(FileActivatedEventArgs e) { LogThis(); BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) { LogThis(); BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) { LogThis(); BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnSearchActivated(SearchActivatedEventArgs e) { LogThis(); BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnShareTargetActivated(ShareTargetActivatedEventArgs e) { LogThis(); BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnLaunched(LaunchActivatedEventArgs e) { LogThis(); BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Launch); }
        protected override sealed void OnBackgroundActivated(BackgroundActivatedEventArgs e) { LogThis(); MessengerService.Send(new Messages.BackgroundActivatedMessage { EventArgs = e }); }
        protected override sealed void OnWindowCreated(WindowCreatedEventArgs e) { LogThis(); BootStrapperStrategy.OnWindowCreated(e); }

        // clean up the object API

        public sealed override bool Equals(object obj) => base.Equals(obj);
        public sealed override int GetHashCode() => base.GetHashCode();
        public sealed override string ToString() => base.ToString();
    }
}
