using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Template10.Services;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Template10
{
    public abstract partial class ApplicationTemplate : IApplicationBase
    {
        public static new ApplicationTemplate Current => (ApplicationTemplate)Application.Current;
        private static readonly SemaphoreSlim _startSemaphore = new SemaphoreSlim(1, 1);
        public const string NavigationServiceParameterName = "navigationService";
        private readonly bool _logStartingEvents = false;

        public ApplicationTemplate()
        {
            InternalInitialize();
            _logger.Log("[App.Constructor()]", Category.Info, Priority.None);
            (this as IApplicationEvents).WindowCreated += (s, e) =>
            {
                GestureService.SetupForCurrentView(e.Window.CoreWindow);
            };
            base.Suspending += async (s, e) =>
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Suspend_Data"))
                {
                    ApplicationData.Current.LocalSettings.Values.Remove("Suspend_Data");
                }
                ApplicationData.Current.LocalSettings.Values.Add("Suspend_Data", DateTime.Now.ToString());
                var deferral = e.SuspendingOperation.GetDeferral();
                try
                {
                    OnSuspending();
                    await OnSuspendingAsync();
                }
                finally
                {
                    deferral.Complete();
                }
            };
            base.Resuming += async (s, e) =>
            {
                await InternalStartAsync(new StartArgs(ResumeArgs.Create(ApplicationExecutionState.Suspended), StartKinds.Resume));
            };
        }

        private IContainerExtension _containerExtension;
        public IContainerProvider Container => _containerExtension;

        private void InternalInitialize()
        {
            // don't forget there is no logger yet
            if (_logStartingEvents)
            {
                _logger.Log($"{nameof(ApplicationTemplate)}.{nameof(InternalInitialize)}", Category.Info, Priority.None);
            }

            // dependecy injection
            _containerExtension = CreateContainerExtension();
            if (_containerExtension is IContainerRegistry registry)
            {
                registry.RegisterSingleton<ILoggerFacade, DebugLogger>();
                registry.RegisterSingleton<IEventAggregator, EventAggregator>();
                RegisterRequiredTypes(registry);
            }

            Debug.WriteLine("[App.RegisterTypes()]");
            RegisterTypes(_containerExtension as IContainerRegistry);

            Debug.WriteLine("Dependency container has just been finalized.");
            _containerExtension.FinalizeExtension();

            // now we can start logging instead of debug/write
            _logger = Container.Resolve<ILoggerFacade>();

            // finalize the application
            ConfigureViewModelLocator();
        }

        private static int _initialized = 0;
        private ILoggerFacade _logger;

        private void CallOnInitializedOnce()
        {
            // don't forget there is no logger yet
            if (_logStartingEvents)
            {
                _logger.Log($"{nameof(ApplicationTemplate)}.{nameof(CallOnInitializedOnce)}", Category.Info, Priority.None);
            }

            // once and only once, ever
            if (Interlocked.Increment(ref _initialized) == 1)
            {
                _logger.Log("[App.OnInitialize()]", Category.Info, Priority.None);
                OnInitialized();
            }
        }

        private async Task InternalStartAsync(StartArgs startArgs)
        {
            await _startSemaphore.WaitAsync();
            if (_logStartingEvents)
            {
                _logger.Log($"{nameof(ApplicationTemplate)}.{nameof(InternalStartAsync)}({startArgs})", Category.Info, Priority.None);
            }

            try
            {
                CallOnInitializedOnce();
                TestResuming(startArgs);
                _logger.Log($"[App.OnStart(startKind:{startArgs.StartKind}, startCause:{startArgs.StartCause})]", Category.Info, Priority.None);
                OnStart(startArgs);
                _logger.Log($"[App.OnStartAsync(startKind:{startArgs.StartKind}, startCause:{startArgs.StartCause})]", Category.Info, Priority.None);
                await OnStartAsync(startArgs);
                Window.Current.Activate();
            }
            finally
            {
                _startSemaphore.Release();
            }
        }

        private static void TestResuming(StartArgs startArgs)
        {
            if (startArgs.Arguments is ILaunchActivatedEventArgs e
                && e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Suspend_Data"))
                {
                    ApplicationData.Current.LocalSettings.Values.Remove("Suspend_Data");
                    startArgs.Arguments = ResumeArgs.Create(ApplicationExecutionState.Terminated);
                    startArgs.StartKind = StartKinds.Resume;
                }
            }
        }

        private static DateTime? SuspendData
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values.TryGetValue("Suspend_Data", out var value)
                    && value != null
                    && DateTime.TryParse(value.ToString(), out var date))
                {
                    return date;
                }
                else
                {
                    return null;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["Suspend_Data"] = value;
        }

        #region overrides

        public virtual void OnSuspending() { /* empty */ }

        public virtual Task OnSuspendingAsync()
        {
            return Task.CompletedTask;
        }

        public abstract void RegisterTypes(IContainerRegistry container);

        public virtual void OnInitialized() { /* empty */ }

        public virtual void OnStart(StartArgs args) {  /* empty */ }

        public virtual Task OnStartAsync(StartArgs args)
        {
            return Task.CompletedTask;
        }

        public virtual void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
            {
                return _containerExtension.ResolveViewModelForView(view, type);
            });
        }

        public abstract IContainerExtension CreateContainerExtension();

        protected virtual void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            // don't forget there is no logger yet
            Debug.WriteLine($"{nameof(ApplicationTemplate)}.{nameof(RegisterRequiredTypes)}()");
        }

        #endregion
    }
}
