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
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.LayoutMetricsChanged += (s, e) =>
            {
                AppTitle.Margin = new Thickness(CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset + 12, 8, 0, 0);
            };
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
