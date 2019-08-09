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
using Template10.Core.Services;
using Template10.Ioc;
using Template10.Navigation;
using Template10.Services;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                WindowService.Register(e.Window);
                GestureService.SetupWindowListeners(e.Window.CoreWindow);
            };

            CoreApplication.Exiting += (s, e) =>
            {
                var stopArgs = new StopArgs(StopKind.CoreApplicationExiting) { CoreApplicationEventArgs = e };
                OnStop(stopArgs);
                OnStopAsync(stopArgs).RunSynchronously();
            };

            // this is a timing problem
            //Window.Current.Closed += (s, e) =>
            //{
            //    OnStop(new StopArgs(StopKind.CoreWindowClosed) { CoreWindowEventArgs = e });
            //    OnStopAsync(new StopArgs(StopKind.CoreWindowClosed) { CoreWindowEventArgs = e }).RunSynchronously();
            //};

            // this is a timing problem
            //Windows.UI.Core.Preview.SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += async (s, e) =>
            //{
            //    var deferral = e.GetDeferral();
            //    try
            //    {
            //        OnStop(new StopArgs(StopKind.CloseRequested) { CloseRequestedPreviewEventArgs = e });
            //        await OnStopAsync(new StopArgs(StopKind.CloseRequested) { CloseRequestedPreviewEventArgs = e });
            //    }
            //    finally
            //    {
            //        deferral.Complete();
            //    }
            //};

            base.Suspending += async (s, e) =>
            {
                SuspensionUtilities.SetSuspendDate(DateTime.Now);
                var deferral = e.SuspendingOperation.GetDeferral();
                try
                {
                    var stopArgs = new StopArgs(StopKind.Suspending) { SuspendingEventArgs = e };
                    OnStop(stopArgs);
                    await OnStopAsync(stopArgs);
                }
                finally
                {
                    deferral.Complete();
                }
            };

            base.Resuming += async (s, e) =>
            {
                var resumeArgs = ResumeArgs.Create(ApplicationExecutionState.Suspended);
                var startArgs = new StartArgs(resumeArgs, StartKinds.ResumeInMemory)
                {
                    StartKind = StartKinds.Activate
                };
                await InternalStartAsync(startArgs);
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
                RegisterInternalTypes(registry);
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

        private void CallOnInitializedOnlyOnce()
        {
            // don't forget there is no logger yet
            if (_logStartingEvents)
            {
                _logger.Log($"{nameof(ApplicationTemplate)}.{nameof(CallOnInitializedOnlyOnce)}", Category.Info, Priority.None);
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
                CallOnInitializedOnlyOnce();

                if (SuspensionUtilities.IsResuming(startArgs, out var resumeArgs))
                {
                    startArgs.StartKind = StartKinds.ResumeFromTerminate;
                    startArgs.Arguments = resumeArgs;
                }

                _logger.Log($"[App.OnStart(startKind:{startArgs.StartKind}, startCause:{startArgs.StartCause})]", Category.Info, Priority.None);
                OnStart(startArgs);

                _logger.Log($"[App.OnStartAsync(startKind:{startArgs.StartKind}, startCause:{startArgs.StartCause})]", Category.Info, Priority.None);
                await OnStartAsync(startArgs);

                // this is redundant, but a friendly add-on
                Window.Current.Activate();
            }
            finally
            {
                _startSemaphore.Release();
            }
        }

        #region overrides

        public virtual void OnStop(IStopArgs stopArgs) { /* empty */ }

        public virtual Task OnStopAsync(IStopArgs stopArgs) => Task.CompletedTask;

        public abstract void RegisterTypes(IContainerRegistry container);

        public virtual void OnInitialized() { /* empty */ }

        public virtual void OnStart(IStartArgs args) {  /* empty */ }

        public virtual Task OnStartAsync(IStartArgs args)
        {
            return Task.CompletedTask;
        }

        public virtual void ConfigureViewModelLocator()
        {
            // this is a testability method
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
            {
                return _containerExtension.ResolveViewModelForView(view, type);
            });
        }

        public abstract IContainerExtension CreateContainerExtension();

        protected virtual void RegisterInternalTypes(IContainerRegistry containerRegistry)
        {
            // don't forget there is no logger yet
            Debug.WriteLine($"{nameof(ApplicationTemplate)}.{nameof(RegisterInternalTypes)}()");
        }

        #endregion
    }
}
