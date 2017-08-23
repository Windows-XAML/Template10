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

            Current = this;

            CreateContainer();

            try { var c = Central.ContainerService; }
            catch { throw new Exception($"IContainerService is required but " +
                $"is not registered in DI."); }

            DefaultDependencies();

            RegisterDependencies();

            try { var m = Central.MessengerService; }
            catch { throw new Exception($"IMessengerService is required but " +
                $"is not registered in DI."); }

#if DEBUG
            // test DI
            var sservice = Container.Resolve<ISessionState>();
            var lservice = Container.Resolve<ILoggingService>();
            var xservice = Container.Resolve<ISerializationService>();
            var bservice = Container.Resolve<IBackButtonService>();
            var kservice = Container.Resolve<IKeyboardService>();
            var gservice = Container.Resolve<IGestureService>();
            var rservice = Container.Resolve<IResourceService>();
            var bstrategy = Container.Resolve<IBootStrapperStrategy>();
            var lstrategy = Container.Resolve<ILifecycleStrategy>();
            var sstrategy = Container.Resolve<INavStateStrategy>();
            var tstrategy = Container.Resolve<ITitleBarStrategy>();
            var estrategy = Container.Resolve<IExtendedSessionStrategy>();
            var astrategy = Container.Resolve<IViewModelActionStrategy>();
            var rstrategy = Container.Resolve<IViewModelResolutionStrategy>();
#endif

            LogThis();

            // forward methods

            BootStrapperStrategy.OnStartAsyncDelegate = OnStartAsync;
            BootStrapperStrategy.OnInitAsyncDelegate = OnInitializeAsync;
            BootStrapperStrategy.CreateSpashDelegate = CreateSpash;
            BootStrapperStrategy.CreateRootElementDelegate = CreateRootElement;

            // setup events

            Central.MessengerService.Subscribe<WindowCreatedMessage>(this, AfterFirstWindowCreated);
            base.Resuming += BootStrapperStrategy.HandleResuming;
            base.Suspending += BootStrapperStrategy.HandleSuspending;
            base.EnteredBackground += BootStrapperStrategy.HandleEnteredBackground;
            base.LeavingBackground += BootStrapperStrategy.HandleLeavingBackground;
            base.UnhandledException += BootStrapperStrategy.HandleUnhandledException;
        }

        private void AfterFirstWindowCreated(WindowCreatedMessage obj)
        {
            // unsubscribe so this is only called a single time
            Central.MessengerService.Unsubscribe<WindowCreatedMessage>(this, AfterFirstWindowCreated);

            // these are the things delayed until after the first window is created
            Container.Resolve<IBackButtonService>().Setup();
            Container.Resolve<ITitleBarStrategy>().Update();
        }

        // redirected properties

        private IBootStrapperStrategy BootStrapperStrategy => Container.Resolve<IBootStrapperStrategy>();
        public IContainerService Container => Services.Container.ContainerService.Default;
        public IMessengerService MessengerService => Central.MessengerService;
        public INavigationService NavigationService => Navigation.NavigationService.Default;
        public IDispatcherEx Dispatcher => WindowEx.GetDefault().Dispatcher;
        public ISessionState SessionState => Central.SessionState;

        // net-new properties 

        public new static IBootStrapper Current { get; internal set; }

        // implementation methods

        public virtual Task OnInitializeAsync() => Task.CompletedTask;
        public abstract Task OnStartAsync(IStartArgsEx e);
        public virtual UIElement CreateRootElement(IStartArgsEx e) => null;
        public virtual UIElement CreateSpash(SplashScreen e) => null;
        public abstract IContainerService CreateContainer();
        public abstract void RegisterDependencies();
        void DefaultDependencies()
        {
            // services
            Container.Register<ISessionState, SessionState>();
            Container.Register<ILoggingService, LoggingService>();
            Container.Register<ISerializationService, JsonSerializationService>();
            Container.Register<IBackButtonService, BackButtonService>();
            Container.Register<IKeyboardService, KeyboardService>();
            Container.Register<IGestureService, GestureService>();
            Container.Register<IResourceService, ResourceService>();

            // strategies
            Container.RegisterInstance<IBootStrapperShared>(this);
            Container.Register<IBootStrapperStrategy, DefaultBootStrapperStrategy>();
            Container.Register<ILifecycleStrategy, DefaultLifecycleStrategy>();
            Container.Register<INavStateStrategy, DefaultNavStateStrategy>();
            Container.Register<ITitleBarStrategy, DefaultTitleBarStrategy>();
            Container.Register<IExtendedSessionStrategy, DefaultExtendedSessionStrategy>();
            Container.Register<IViewModelActionStrategy, DefaultViewModelActionStrategy>();
            Container.Register<IViewModelResolutionStrategy, DefaultViewModelResolutionStrategy>();
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
        protected override sealed void OnBackgroundActivated(BackgroundActivatedEventArgs e) { LogThis(); MessengerService.Send(new Messages.BackgroundActivatedMessage { EventArgs = e }); }
        protected override sealed void OnWindowCreated(WindowCreatedEventArgs e) { LogThis(); BootStrapperStrategy.OnWindowCreated(e); }

        // clean up the object API

        public sealed override bool Equals(object obj) => base.Equals(obj);
        public sealed override int GetHashCode() => base.GetHashCode();
        public sealed override string ToString() => base.ToString();
    }
}
