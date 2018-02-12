using Prism.Windows.Services;
using win = Windows;
using Prism.Windows.Navigation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Prism.Windows.Services.DialogService;
using Prism.Windows.Utilities;
using System.Linq;
using System;

namespace PrismSample.Views
{
    public sealed partial class ShellPage : Page
    {
        private IGestureService _gestureService;
        private IDialogService _dialogService;

        public ShellPage(IGestureService gestureService, IDialogService dialogService)
        {
            InitializeComponent();

            if (win.ApplicationModel.DesignMode.DesignModeEnabled
                || win.ApplicationModel.DesignMode.DesignMode2Enabled)
            {
                return;
            }

            _gestureService = gestureService;
            _dialogService = dialogService;

            ShellView.Start();

            ShellView.Loaded += (s, e) =>
            {
                SetupGestures();
                SetupBackButton();
                SetupControlBox();
            };
        }

        private void SetupControlBox()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];
        }

        private void SetupBackButton()
        {
            var children = XamlUtilities.RecurseChildren(ShellView);
            var grids = children.OfType<Grid>();
            var grid = grids.Single(x => x.Name == "TogglePaneTopPadding");
            grid.Visibility = Visibility.Collapsed;

            grid = grids.Single(x => x.Name == "ContentPaneTopPadding");
            grid.RegisterPropertyChangedCallback(HeightProperty, (s, args) =>
            {
                if (grid.Height != 44d)
                {
                    grid.Height = 44d;
                }
            });
            grid.Height = 44d;

            var buttons = children.OfType<Button>();
            var button = buttons.Single(x => x.Name == "TogglePaneButton");
            button.RegisterPropertyChangedCallback(MarginProperty, (s, args) =>
            {
                if (button.Margin.Top != 0)
                {
                    button.Margin = new Thickness(0, 0, 0, 32);
                }
            });
            button.Margin = new Thickness(0, 0, 0, 32);
            button.Focus(FocusState.Programmatic);

            var parent = button.Parent as Grid;
            var backButton = new Button
            {
                Name = "BackButton",
                Content = new SymbolIcon
                {
                    Symbol = Symbol.Back,
                    IsHitTestVisible = false
                },
                Style = Resources["PaneToggleButtonStyle"] as Style,
            };
            parent.Children.Insert(1, backButton);
            ShellView.NavigationService.CanGoBackChanged += (s, args) =>
            {
                backButton.IsEnabled = ShellView.NavigationService.CanGoBack();
            };
            backButton.Click += (s, args) =>
            {
                _gestureService.RaiseBackRequested();
            };
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
