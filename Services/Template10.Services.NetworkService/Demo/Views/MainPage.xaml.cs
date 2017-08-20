using System;
using Template10.Services.Network;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Template10.Demo.NetworkService
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
