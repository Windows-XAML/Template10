using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

/*
 * If you can't find 'Template10.Services.NetworkService' package on Nuget, you can install it locally.
 * Open Nuget Package Manager Console:   Tools -> Nuget Package Manager -> Package Manager Console
 * and with the Default Project set to this demo project, type in and Enter at the prompt:
 *
 *  Install-Package Template10.Services.NetworkService.1.0.0.nupkg -Source ..\Nuget
*/

namespace Template10.Services.NetworkService.Demo
{
    public sealed partial class MainPage : Page
    {
        #region Fields

        private NetworkAvailableService nas;
        private SolidColorBrush green;
        private SolidColorBrush gray;
        private Action<ConnectionTypes> actionHandler;

        #endregion

        #region Constructors
        public MainPage()
        {
            this.InitializeComponent();

            nas = new NetworkAvailableService();
            green = new SolidColorBrush(Colors.Green);
            gray = new SolidColorBrush(Colors.Gray);

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

        #endregion

        #region Properties
        private SolidColorBrush _localNetworkFontIconForeground;
        public SolidColorBrush LocalNetworkFontIconForeground
        {
            get { return _localNetworkFontIconForeground; }
            set { Set(ref _localNetworkFontIconForeground, value); }
        }

        private SolidColorBrush _internetFontIconForeground;
        public SolidColorBrush InternetFontIconForeground
        {
            get { return _internetFontIconForeground; }
            set { Set(ref _internetFontIconForeground, value); }
        }

        #endregion

        #region Event handlers
        private async void UpdateNetworkStatus()
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var isInternetAvail = await nas.IsInternetAvailable();
                var isLocalNetAvail = await nas.IsNetworkAvailable();
                InternetFontIconForeground = (isInternetAvail) ? green : gray;
                LocalNetworkFontIconForeground = (isLocalNetAvail || isInternetAvail) ? green : gray;

            });

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UpdateNetworkStatus();
        }

        #endregion

    }
}
