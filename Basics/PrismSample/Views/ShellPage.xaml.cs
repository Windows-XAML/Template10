using Prism.Windows.Services;
using Prism.Windows.Navigation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Prism.Windows.Services.DialogService;

namespace PrismSample.Views
{
    public sealed partial class ShellPage : Page
    {
        private IGestureService _gestureService;
        private IDialogService _dialogService;

        public ShellPage(IGestureService gestureService, IDialogService dialogService)
        {

            _gestureService = gestureService;
            _dialogService = dialogService;

            InitializeComponent();

            ShellView.Start();
            SetupGestures();
            SetupTitleBar();
        }

        private void SetupGestures()
        {
            _gestureService.BackRequested += async (s, e) => await ShellView.NavigationService.GoBackAsync();
            _gestureService.ForwardRequested += async (s, e) => await ShellView.NavigationService.GoForwardAsync();
            _gestureService.RefreshRequested += async (s, e) => await ShellView.NavigationService.RefreshAsync();
            _gestureService.MenuRequested += (s, e) => ShellView.IsPaneOpen = true;
            _gestureService.SearchRequested += (s, e) =>
            {
                ShellView.IsPaneOpen = true;
                ShellView.AutoSuggestBox?.Focus(FocusState.Programmatic);
            };
        }

        private void SetupTitleBar()
        {
            var viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.ButtonBackgroundColor = Colors.Transparent;
            viewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            viewTitleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            void UpdateAppTitle()
            {
                var full = ApplicationView.GetForCurrentView().IsFullScreenMode;
                var left = 12 + (full ? 0 : CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset);
                AppTitle.Margin = new Thickness(left, 8, 0, 0);
            }

            Window.Current.CoreWindow.SizeChanged += (s, e) => UpdateAppTitle();
            coreTitleBar.LayoutMetricsChanged += (s, e) => UpdateAppTitle();

            ShellView.RegisterPropertyChangedCallback(NavigationView.IsPaneOpenProperty, (s, e) =>
            {
                AppTitle.Visibility = ShellView.IsPaneOpen ? Visibility.Visible : Visibility.Collapsed;
            });
        }

        private async void ProfileItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await _dialogService.AlertAsync("Show user profile.");
        }

        private void FeedbackItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // https://docs.microsoft.com/en-us/windows/uwp/monetize/launch-feedback-hub-from-your-app
        }
    }
}
