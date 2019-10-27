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
using Prism.Events;

namespace Sample.Views
{
    public sealed partial class ShellPage : Page
    {
        private readonly IGestureService _gestureService;
        private readonly IDialogService _dialogService;
        private readonly IDataService _dataService;
        private readonly IEventAggregator _eventAggregator;

        public ShellPage(IDialogService dialogService, IDataService dataService, IEventAggregator eventAggregator)
        {
            InitializeComponent();

            if (win.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }

            _dialogService = dialogService;
            _dataService = dataService;

            _gestureService = GestureService.GetForCurrentView();
            _gestureService.MenuRequested += (s, e) => ShellView.IsPaneOpen = true;
            _gestureService.SearchRequested += (s, e) =>
            {
                ShellView.IsPaneOpen = true;
                ShellView.AutoSuggestBox?.Focus(FocusState.Programmatic);
            };

            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<Messages.ShowEditorMessage>().Subscribe(item =>
            {
                EditSplitView.IsPaneOpen = true;
                SideEditView.DataContext = item;
            });

            ShellView.Initialize();
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
            await ShellView.NavigationService.NavigateAsync("SearchPage", ("SearchTerm", sender.Text = args.SelectedItem.ToString()));
        }

        private async void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            await ShellView.NavigationService.NavigateAsync("SearchPage", ("SearchTerm", sender.Text));
        }

        private async void Template10_Click(object sender, RoutedEventArgs e)
        {
            await win.System.Launcher.LaunchUriAsync(new Uri("http://aka.ms/template10"));
        }
    }
}
