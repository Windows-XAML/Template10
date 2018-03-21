using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.Compression;
using Template10.Services.Dialog;
using Template10.Services.File;
using Template10.Services.Marketplace;
using Template10.Services.Nag;
using Template10.Services.Network;
using Template10.Services.Resources;
using Template10.Services.Secrets;
using Template10.Services.Settings;
using Template10.Services.Web;

namespace Template10.Utilities
{
    public static class Extensions
    {
        /// <summary>
        /// Automatically registers: 
        ///     ICompressionService, IDialogService, 
        ///     IFileService, IMarketplaceService, 
        ///     INagService, INetworkService, IResourceService, 
        ///     ISecretService, ISettingsHelper, IWebApiService
        /// </summary>
        /// <remarks>
        /// Don't forget that you can register these manually. This method is a convience method. 
        /// There is no real cost to registering extra services.
        /// </remarks>
        /// <param name="registry">IContainerRegistry</param>
        public static void RegisterTemplate10Services(this IContainerRegistry registry)
        {
            registry.RegisterSingleton<ICompressionService, CompressionService>();
            registry.RegisterSingleton<IDialogService, DialogService>();
            registry.RegisterSingleton<IFileService, FileService>();
            registry.RegisterSingleton<IMarketplaceService, MarketplaceService>();
            registry.RegisterSingleton<INagService, NagService>();
            registry.RegisterSingleton<INetworkService, NetworkService>();
            registry.RegisterSingleton<IResourceService, ResourceService>();
            registry.RegisterSingleton<ISecretService, SecretService>();
            registry.RegisterSingleton<ISettingsHelper, SettingsHelper>();
            registry.RegisterSingleton<IWebApiService, WebApiService>();
        }
    }
}
