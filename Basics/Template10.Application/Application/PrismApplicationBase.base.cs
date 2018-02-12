using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Navigation;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Prism.Windows.Services;
using Prism.Windows.Services.Compression;
using Prism.Windows.Services.DialogService;
using Prism.Windows.Services.FileService;
using Prism.Windows.Services.Marketplace;
using Prism.Windows.Services.Nag;
using Prism.Windows.Services.Network;
using Prism.Windows.Services.Resources;
using Prism.Windows.Services.Secrets;
using Prism.Windows.Services.Serialization;
using Prism.Windows.Services.Settings;
using Prism.Windows.Services.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Prism.Windows
{
    public interface IPrismApplicationBase
    {
        IContainerExtension CreateContainer();
        void RegisterRequiredTypes(IContainerRegistry container);
        void RegisterTypes(IContainerRegistry container);
        void OnInitialize(IMvvmLocator locator);
        void OnStart(StartArgs args, StartKinds activate);
        Task OnStartAsync(StartArgs args, StartKinds activate);
        ISessionState SessionState { get; }
    }

    public abstract partial class PrismApplicationBase : IPrismApplicationBase
    {
        private static SemaphoreSlim _startSemaphore = new SemaphoreSlim(1, 1);

        internal static IContainerExtension Container { get; private set; }

        public PrismApplicationBase()
        {
            InternalInitialize();
        }

        public ISessionState SessionState => Container.Resolve<ISessionState>();

        public abstract IContainerExtension CreateContainer();

        public virtual void RegisterRequiredTypes(IContainerRegistry container)
        {
            // identical across Prism

            container.RegisterSingleton<ILoggerFacade, EmptyLogger>();
            container.RegisterSingleton<IEventAggregator, EventAggregator>();

            // similar across Prism

            container.RegisterSingleton<IGestureService, GestureService>();
            container.RegisterSingleton<IMvvmLocator, MvvmLocator>();
            container.RegisterSingleton<IPageRegistry, PageRegistry>();

            // unique to Prism.Windows

            container.RegisterSingleton<ICompressionService, CompressionService>();
            container.RegisterSingleton<IDialogService, DialogService>();
            container.RegisterSingleton<IFileService, FileService>();
            container.RegisterSingleton<IMarketplaceService, MarketplaceService>();
            container.RegisterSingleton<INagService, NagService>();
            container.RegisterSingleton<INetworkService, NetworkService>();
            container.RegisterSingleton<IResourceService, ResourceService>();
            container.RegisterSingleton<ISecretService, SecretService>();
            container.RegisterSingleton<ISerializationService, NullSerializationService>();
            container.RegisterSingleton<ISettingsHelper, SettingsHelper>();
            container.RegisterSingleton<IWebApiService, WebApiService>();
        }

        public abstract void RegisterTypes(IContainerRegistry container);

        public virtual void OnInitialize(IMvvmLocator locator) { /* empty */ }

        public virtual void OnStart(StartArgs args, StartKinds activate) {  /* empty */ }

        public virtual Task OnStartAsync(StartArgs args, StartKinds activate) => Task.CompletedTask;

        private void InternalInitialize()
        {
            Debug.WriteLine($"{nameof(PrismApplicationBase)}.{nameof(InternalInitialize)}");

            Container = CreateContainer();
            RegisterRequiredTypes(Container);
            RegisterTypes(Container);
            OnInitialize(Container.Resolve<IMvvmLocator>());
        }

        private async Task InternalStartAsync(StartArgs startArgs, StartKinds activate)
        {
            Debug.WriteLine($"{nameof(PrismApplicationBase)}.{nameof(InternalStartAsync)}({startArgs.Arguments}, {activate})");

            await _startSemaphore.WaitAsync();

            try
            {
                Window.Current.Activate();

                try
                {
                    Debug.WriteLine($"{nameof(PrismApplicationBase)}.{nameof(InternalStartAsync)}.{nameof(OnStart)}({startArgs.Arguments}, {activate})");
                    OnStart(startArgs, activate);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{nameof(PrismApplicationBase)}.{nameof(InternalStartAsync)} exception: {ex}/{ex.Message})");
                    Debugger.Break();
                }

                try
                {
                    Debug.WriteLine($"{nameof(PrismApplicationBase)}.{nameof(InternalStartAsync)}.{nameof(OnStartAsync)}({startArgs.Arguments}, {activate})");
                    await OnStartAsync(startArgs, activate);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{nameof(PrismApplicationBase)}.{nameof(OnStartAsync)} exception: {ex}/{ex.Message})");
                    Debugger.Break();
                }
            }
            finally
            {
                _startSemaphore.Release();
            }
        }
    }
}
