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

namespace Template10
{
    public abstract class BootStrapper<T> : BootStrapper where T : IContainerService
    {
        public BootStrapper(IBootStrapperStrategy strategy = null)
            : base(Activator.CreateInstance<T>(), strategy)
        {
            // empty
        }
    }

    public abstract partial class BootStrapper : ILoggable
    {
        ILoggingService ILoggable.LoggingService { get; set; }
        void LogThis(string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
            => (this as ILoggable).LogThis(text, severity, caller: $"{GetType()}.{caller}");
        void ILoggable.LogThis(string text, Severities severity, string caller)
            => (this as ILoggable).LoggingService.WriteLine(text, severity, caller: $"{GetType()}.{caller}");
    }

    public abstract partial class BootStrapper : Application, IBootStrapper
    {
        IContainerService _container;
        internal BootStrapper(IContainerService container, IBootStrapperStrategy strategy)
            : this(strategy)
        {
            _container = container;
        }

        public BootStrapper(IBootStrapperStrategy strategy = null)
        {
            Current = this;

            RegisterDependencyInjection(strategy);

            _BootStrapperStrategy.OnStartAsyncDelegate = OnStartAsync;
            _BootStrapperStrategy.OnInitAsyncDelegate = OnInitializeAsync;
            _BootStrapperStrategy.CreateSpashAsyncDelegate = CreateSpashAsync;
            _BootStrapperStrategy.CreateRootElementAsyncDelegate = CreateRootElementAsync;

            base.Resuming += _BootStrapperStrategy.HandleResuming;
            base.Suspending += _BootStrapperStrategy.HandleSuspending;
            base.EnteredBackground += _BootStrapperStrategy.HandleEnteredBackground;
            base.LeavingBackground += _BootStrapperStrategy.HandleLeavingBackground;
            base.UnhandledException += _BootStrapperStrategy.HandleUnhandledException;
        }

        void RegisterDependencyInjection(IBootStrapperStrategy strategy)
        {
            _container = _container ?? new Services.Container.UnityContainerService();
            _container.Register<Core.ISessionState, Core.SessionState>();
            _container.Register<Services.Logging.ILoggingService, Services.Logging.LoggingService>();
            _container.Register<Services.Messenger.IMessengerService, Services.Messenger.MvvmLightMessengerService>();
            _container.Register<Services.Serialization.ISerializationService, Services.Serialization.JsonSerializationService>();
            _container.Register<Services.BackButtonService.IBackButtonService, Services.BackButtonService.BackButtonService>();
            if (strategy == null)
            {
                _container.Register<Strategies.IBootStrapperStrategy, DefaultBootStrapperStrategy>();
            }
            else
            {
                _container.Register<Strategies.IBootStrapperStrategy>(strategy);
            }
            _container.Register<Strategies.ILifecycleStrategy, Strategies.DefaultLifecycleStrategy>();
            _container.Register<Strategies.IStateStrategy, Strategies.DefaultStateStrategy>();
            _container.Register<Strategies.ITitleBarStrategy, Strategies.DefaultTitleBarStrategy>();
            _container.Register<Strategies.IExtendedSessionStrategy, Strategies.DefaultExtendedSessionStrategy>();
            _container.Register<Strategies.IViewModelActionStrategy, Strategies.DefaultViewModelActionStrategy>();
            _container.Register<Strategies.IViewModelResolutionStrategy, Strategies.DefaultViewModelResolutionStrategy>();
        }

        // redirected properties

        private IBootStrapperStrategy _BootStrapperStrategy => ContainerService.Resolve<IBootStrapperStrategy>();
        public IContainerService ContainerService => Services.Container.ContainerService.Default;
        public Services.Messenger.IMessengerService MessengerService => ContainerService.Resolve<Services.Messenger.IMessengerService>();
        public INavigationService NavigationService => Navigation.NavigationService.Default;
        public IDispatcherEx Dispatcher => WindowEx.GetDefault().Dispatcher;
        public ISessionState SessionState => ContainerService.Resolve<ISessionState>();

        // net-new properties 

        public new static BootStrapper Current { get; internal set; }

        // implementation methods

        public virtual Task OnInitializeAsync() => null;
        public abstract Task OnStartAsync(IStartArgsEx e);
        public virtual Task<UIElement> CreateRootElementAsync(IStartArgsEx e) => null;
        public virtual Task<UIElement> CreateSpashAsync(SplashScreen e) => null;

        // hidden events

        private new event EventHandler<object> Resuming;
        private new event SuspendingEventHandler Suspending;
        private new event UnhandledExceptionEventHandler UnhandledException;
        private new event EnteredBackgroundEventHandler EnteredBackground;
        private new event LeavingBackgroundEventHandler LeavingBackground;

        // hidden overrides

        protected override sealed void OnActivated(IActivatedEventArgs e) { LogThis(); _BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) { LogThis(); _BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnFileActivated(FileActivatedEventArgs e) { LogThis(); _BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) { LogThis(); _BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) { LogThis(); _BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnSearchActivated(SearchActivatedEventArgs e) { LogThis(); _BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnShareTargetActivated(ShareTargetActivatedEventArgs e) { LogThis(); _BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Activate); }
        protected override sealed void OnLaunched(LaunchActivatedEventArgs e) { LogThis(); _BootStrapperStrategy.StartOrchestrationAsync(e, StartKinds.Launch); }
        protected override sealed void OnBackgroundActivated(BackgroundActivatedEventArgs e) { LogThis(); MessengerService.Send(new Messages.BackgroundActivatedMessage { EventArgs = e }); }
        protected override sealed void OnWindowCreated(WindowCreatedEventArgs e) { LogThis(); _BootStrapperStrategy.OnWindowCreated(e); }

        // clean up the object API

        public sealed override bool Equals(object obj) => base.Equals(obj);
        public sealed override int GetHashCode() => base.GetHashCode();
        public sealed override string ToString() => base.ToString();
    }
}
