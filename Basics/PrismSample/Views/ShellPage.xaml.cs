using Template10.Application.Services;
using Template10.Navigation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PrismSample.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            InitializeComponent();
            ShellView.Start();
            SetupGestures();
            SetupTitleBar();
        }

        private void SetupGestures()
        {
            GestureService.BackRequested += async (s, e) => await ShellView.NavigationService.GoBackAsync();
            GestureService.ForwardRequested += async (s, e) => await ShellView.NavigationService.GoForwardAsync();
            GestureService.RefreshRequested += async (s, e) => await ShellView.NavigationService.RefreshAsync();
            GestureService.MenuRequested += (s, e) => ShellView.IsPaneOpen = true;
            GestureService.SearchRequested += (s, e) =>
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

        private void ProfileItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // TODO
        }

        private void FeedbackItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // https://docs.microsoft.com/en-us/windows/uwp/monetize/launch-feedback-hub-from-your-app
        }
    }
}
