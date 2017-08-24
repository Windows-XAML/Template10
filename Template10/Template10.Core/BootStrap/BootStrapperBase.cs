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
using Template10.Messages;
using Template10.Services.Resources;

namespace Template10.BootStrap
{
    public abstract partial class BootStrapperBase : ILoggable
    {
        ILoggingService ILoggable.LoggingService => Central.LoggingService;
        void LogThis(string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
            => (this as ILoggable).LogThis(text, severity, caller: $"{caller}");
        void ILoggable.LogThis(string text, Severities severity, string caller)
            => (this as ILoggable).LoggingService.WriteLine(text, severity, caller: $"{caller}");
    }

    public abstract partial class BootStrapperBase : Application, IBootStrapper
    {
        public BootStrapperBase()
        {
            // start

            CreateDependecyContainer();
            try { var c = Central.ContainerService; }
            catch { throw new Exception($"IContainerService is required but is not defined in DI."); }

            RegisterDefaultDependencies();
            RegisterCustomDependencies();
            try { var m = Central.MessengerService; }
            catch { throw new Exception($"IMessengerService is required but is not registered in DI."); }

#if DEBUG
            // test DI
            var sservice = Central.ContainerService.Resolve<ISessionState>();
            var lservice = Central.ContainerService.Resolve<ILoggingService>();
            var xservice = Central.ContainerService.Resolve<ISerializationService>();
            var bservice = Central.ContainerService.Resolve<IBackButtonService>();
            var kservice = Central.ContainerService.Resolve<IKeyboardService>();
            var gservice = Central.ContainerService.Resolve<IGestureService>();
            var rservice = Central.ContainerService.Resolve<IResourceService>();
            var bstrategy = Central.ContainerService.Resolve<IBootStrapperStrategy>();
            var lstrategy = Central.ContainerService.Resolve<ILifecycleStrategy>();
            var sstrategy = Central.ContainerService.Resolve<INavStateStrategy>();
            var tstrategy = Central.ContainerService.Resolve<ITitleBarStrategy>();
            var estrategy = Central.ContainerService.Resolve<IExtendedSessionStrategy>();
            var astrategy = Central.ContainerService.Resolve<IViewModelActionStrategy>();
            var rstrategy = Central.ContainerService.Resolve<IViewModelResolutionStrategy>();
#endif

            LogThis();

            // forward methods

            BootStrapperStrategy.OnStartAsyncDelegate = OnStartAsync;
            BootStrapperStrategy.OnInitAsyncDelegate = OnInitializeAsync;
            BootStrapperStrategy.CreateSpashDelegate = CreateSpash;
            BootStrapperStrategy.CreateRootElementDelegate = CreateRootElement;

            // setup events

            Central.MessengerService.Subscribe<WindowCreatedMessage>(this, AfterFirstWindowCreated);
            Central.MessengerService.Subscribe<AppVisibilityChangedMessage>(this, (e) => Central.AppVisibility = e.Visibility);
            Central.MessengerService.Subscribe<ExtendedSessionRevokedMessage>(this, (e) =>
            {
                if (e.ExtendedExecutionReason == Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionReason.Unspecified
                    && Central.AppVisibility == AppVisibilities.Foreground)
                {
                    OnClose(new ClosedEventArgs(e.TryToExtendAsync));
                }
            });
            base.Resuming += BootStrapperStrategy.HandleResuming;
            base.Suspending += BootStrapperStrategy.HandleSuspending;
            base.EnteredBackground += (s, e) =>
            {
                Central.AppVisibility = AppVisibilities.Background;
                BootStrapperStrategy.HandleEnteredBackground(s, e);
            };
            base.LeavingBackground += (s, e) =>
            {
                Central.AppVisibility = AppVisibilities.Foreground;
                BootStrapperStrategy.HandleLeavingBackground(s, e); ;
            };
            base.UnhandledException += BootStrapperStrategy.HandleUnhandledException;
        }

        private void AfterFirstWindowCreated(WindowCreatedMessage obj)
        {
            // unsubscribe so this is only called a single time
            Central.MessengerService.Unsubscribe<WindowCreatedMessage>(this, AfterFirstWindowCreated);

            // these are the things delayed until after the first window is created
            Central.ContainerService.Resolve<IBackButtonService>().Setup();
            Central.ContainerService.Resolve<ITitleBarStrategy>().Update();
        }

        // isolated properties

        private IBootStrapperStrategy BootStrapperStrategy 
            => Central.ContainerService.Resolve<IBootStrapperStrategy>();

        // implementation methods

        public virtual void OnClose(ClosedEventArgs e) { }
        public virtual Task OnInitializeAsync() => Task.CompletedTask;
        public abstract Task OnStartAsync(IStartArgsEx e);
        public virtual UIElement CreateRootElement(IStartArgsEx e) => null;
        public virtual UIElement CreateSpash(SplashScreen e) => null;
        public abstract IContainerService CreateDependecyContainer();
        public abstract void RegisterCustomDependencies();
        void RegisterDefaultDependencies()
        {
            // services
            Central.ContainerService.Register<ISessionState, SessionState>();
            Central.ContainerService.Register<ILoggingService, LoggingService>();
            Central.ContainerService.Register<ISerializationService, JsonSerializationService>();
            Central.ContainerService.Register<IBackButtonService, BackButtonService>();
            Central.ContainerService.Register<IKeyboardService, KeyboardService>();
            Central.ContainerService.Register<IGestureService, GestureService>();
            Central.ContainerService.Register<IResourceService, ResourceService>();

            // strategies
            Central.ContainerService.RegisterInstance<IBootStrapperShared>(this);
            Central.ContainerService.Register<IBootStrapperStrategy, DefaultBootStrapperStrategy>();
            Central.ContainerService.Register<ILifecycleStrategy, DefaultLifecycleStrategy>();
            Central.ContainerService.Register<INavStateStrategy, DefaultNavStateStrategy>();
            Central.ContainerService.Register<ITitleBarStrategy, DefaultTitleBarStrategy>();
            Central.ContainerService.Register<IExtendedSessionStrategy, DefaultExtendedSessionStrategy>();
            Central.ContainerService.Register<IViewModelActionStrategy, DefaultViewModelActionStrategy>();
            Central.ContainerService.Register<IViewModelResolutionStrategy, DefaultViewModelResolutionStrategy>();
        }

        // override built-in Application events

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
        protected override sealed void OnBackgroundActivated(BackgroundActivatedEventArgs e) { LogThis(); Central.MessengerService.Send(new Messages.BackgroundActivatedMessage { EventArgs = e }); }
        protected override sealed void OnWindowCreated(WindowCreatedEventArgs e) { LogThis(); BootStrapperStrategy.OnWindowCreated(e); }

        // clean up the object API

        public sealed override bool Equals(object obj) => base.Equals(obj);
        public sealed override int GetHashCode() => base.GetHashCode();
        public sealed override string ToString() => base.ToString();
    }
}
