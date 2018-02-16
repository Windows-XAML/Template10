using Prism.Ioc;
using Prism.Windows.Navigation;
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
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml;

namespace Prism.Windows
{
    public abstract class PrismApplication : PrismApplicationBase
    {
        public PrismApplication()
            : base()
        {
            (this as IPrismApplicationEvents).WindowCreated += (s, e) =>
            {
                Template10.Services.Gesture.GestureService.SetupForCurrentView(e.Window.CoreWindow);
            };
        }

        public sealed override IContainerExtension CreateContainer()
        {
            return new CoreContainerExtension(new ServiceCollection());
        }

        public override void RegisterRequiredTypes(IContainerRegistry container)
        {
            // identical across prism

            container.RegisterSingleton<ILoggerFacade, EmptyLogger>();
            container.RegisterSingleton<IEventAggregator, EventAggregator>();

            // common services

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

            // custom in this project

            container.RegisterSingleton<ISerializationService, NewtonsoftSerializationService>();
        }
    }
}
