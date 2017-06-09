# NetworkAvailableService

The `NetworkAvailableService` provides a very simple interface to allow apps to check the current status of the network available to the device and to be advised when the network availability changes.

````csharp

// Check on the current status to see if access to the Internet is possible.
public async Task<bool> IsInternetAvailable();

// Check on the current status to see if there is any network availability.
public async Task<bool> IsNetworkAvailable();

// Hook into notifications of when the availability changes.
public Action<ConnectionTypes> AvailabilityChanged { get; set; }

````

# Usage Example (See a Demo for details)

````csharp

using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NetworkService.Demo
{
    public sealed partial class MainPage : Page
    {
        private NetworkAvailableService nas;
        private Action<ConnectionTypes> actionHandler;

        public MainPage()
        {
            nas = new NetworkAvailableService();
            Loaded += (s, e) =>
            {
                actionHandler = (action) => UpdateNetworkStatus();
                nas.AvailabilityChanged += actionHandler;
            };

            Unloaded += (s, e) =>
            {
                nas.AvailabilityChanged -= actionHandler;
            };
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           UpdateNetworkStatus();   // get up-to-date status on navigating to page
        }
        
        private async void UpdateNetworkStatus()
        {
            // make sure we're on UI thread to update UI properties
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var isInternetAvail = await nas.IsInternetAvailable();
                var isLocalNetAvail = await nas.IsNetworkAvailable();
                
                // use the above bool flags to update status by setting UI Properties, etc
			
            });
        }
    }
}

````
