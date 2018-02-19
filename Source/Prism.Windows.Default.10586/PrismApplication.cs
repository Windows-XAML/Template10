using Prism.Ioc;
using Prism.Navigation;
using Prism.Logging;
using Prism.Events;
using Template10.Services;
using Template10.Services.Dialog;
using Template10.Services.Compression;
using Template10.Services.File;
using Template10.Services.Marketplace;
using Template10.Services.Nag;
using Template10.Services.Network;
using Template10.Services.Resources;
using Template10.Services.Secrets;
using Template10.Services.Serialization;
using Template10.Services.Settings;
using Template10.Services.Web;
using Windows.UI.Xaml;
using Template10;
using Unity;

namespace Prism
{
    public abstract class PrismApplication : PrismApplicationBase
    {
        public PrismApplication() : base()
        {
            (this as IPrismApplicationEvents).WindowCreated += (s, e) =>
            {
                // doing it this way will make gestures work for each subsequent view
                Template10.Services.Gesture.GestureService.SetupForCurrentView(e.Window.CoreWindow);
            };
        }

        public sealed override IContainerExtension CreateContainer()
        {
            return new UnityContainerExtension(new UnityContainer());
        }

        public override void RegisterTypes(IContainerRegistry container)
        {
            // required for view-models

            container.Register<INavigationService, NavigationService>(NavigationServiceParameterName);

            // standard prism services

            container.RegisterSingleton<ILoggerFacade, EmptyLogger>();
            container.RegisterSingleton<IEventAggregator, EventAggregator>();

            // standard template 10 services

            container.RegisterSingleton<ICompressionService, CompressionService>();
            container.RegisterSingleton<IDialogService, DialogService>();
            container.RegisterSingleton<IFileService, FileService>();
            container.RegisterSingleton<IMarketplaceService, MarketplaceService>();
            container.RegisterSingleton<INagService, NagService>();
            container.RegisterSingleton<INetworkService, NetworkService>();
            container.RegisterSingleton<IResourceService, ResourceService>();
            container.RegisterSingleton<ISecretService, SecretService>();
            container.RegisterSingleton<ISettingsHelper, SettingsHelper>();
            container.RegisterSingleton<IWebApiService, WebApiService>();

            // custom services

            container.RegisterSingleton<ISerializationService, NewtonsoftSerializationService>();
        }
    }
}
