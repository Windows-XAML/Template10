using win = Windows;
using Prism.Navigation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Prism.Utilities;
using System.Linq;
using System;
using Windows.UI.Xaml.Media;
using Template10.Services;
using Template10.Services.Dialog;
using Sample.Services;
using System.Collections.Generic;
using Sample.Models;
using Prism.Services;

namespace Sample.Views
{
    public sealed partial class ShellPage : Page
    {
        private readonly IGestureService _gestureService;
        private readonly IDialogService _dialogService;
        private readonly IDataService _dataService;

        public ShellPage(IDialogService dialogService, IDataService dataService)
        {
            InitializeComponent();

            if (win.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }

            _gestureService = GestureService.GetForCurrentView();
            _dialogService = dialogService;
            _dataService = dataService;

            ShellView.Initialize();
            ShellView.Loaded += (s, e) =>
            {
                SetupGestures();
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

        private void Search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = _dataService.GetSuggestions(sender.Text, 5);
            }
        }

        private async void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            await ShellView.NavigationService.NavigateAsync("/SearchPage", ("SearchTerm", sender.Text = args.SelectedItem.ToString()));
        }

        private async void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            await ShellView.NavigationService.NavigateAsync("/SearchPage", ("SearchTerm", sender.Text));
        }
    }
}
