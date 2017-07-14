using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.NavigationService
{
    public static class NavigationServiceFactory
    {
        /// <summary>
        /// Creates a new NavigationService from the gived Frame to the
        /// WindowWrapper collection. In addition, it optionally will setup the
        /// shell back button to react to the nav of the Frame.
        /// A developer should call this when creating a new/secondary frame.
        /// </summary>
        /// <remarks>
        /// The shell back button should only be setup one time.
        /// </remarks>
        public static async Task<INavigationService> CreateAsync(BackButton backButton, Frame frame = null)
        {
            await CheckAllCacheExpiryAsync();

            var existing = frame.GetNavigationService();
            if (existing != null)
            {
                return existing;
            }

            var service = new NavigationService(frame);
            service.BackButtonHandling = backButton;
            if (backButton == BackButton.Attach)
            {
                frame.RegisterPropertyChangedCallback(Frame.BackStackDepthProperty, (s, args) => BackButtonService.BackButtonService.Instance.UpdateBackButton(service.CanGoBack));
                frame.Navigated += (s, args) => BackButtonService.BackButtonService.Instance.UpdateBackButton(service.CanGoBack);
                BackButtonService.BackButtonService.Instance.BackRequested += async (s, e) => e.Handled = await service.GoBackAsync();
            }

            if (!NavigationServiceHelper.Instances.Any())
            {
                NavigationServiceHelper.Default = service;
            }
            NavigationServiceHelper.Instances.Add(service);

            return service;
        }

        async static Task CheckAllCacheExpiryAsync()
        {
            // this is always okay to check, default or not, 
            // expire any state (based on expiry delta from today)
            foreach (var nav in NavigationServiceHelper.Instances)
            {
                var facade = nav.FrameFacade as IFrameFacadeInternal;
                var state = await facade.GetFrameStateAsync();
                var setting = await state.TryGetCacheDateKeyAsync();

                // default the cache age to very fresh if not known
                var date = setting.Success ? setting.Value : DateTime.MaxValue;
                var cacheAge = DateTime.Now.Subtract(date);

                // clear state in every nav service in every view
                if (cacheAge >= Settings.CacheMaxDuration)
                {
                    await state.ClearAsync();
                }
            }
        }
    }
}
