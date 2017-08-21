﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Extensions;
using Windows.UI.Xaml.Controls;
using Template10.Navigation;

namespace Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Value = "Designtime value";
            }
        }

        static string _Value = "Gas";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public async override Task OnNavigatedToAsync(INavigatedToParameters parameter)
        {
            if (parameter.NavigationMode == NavMode.Back | parameter.Resuming)
            {
                var item = await parameter.ToNavigationInfo.PageState.TryGetAsync<string>(nameof(Value));
                if (item.Success)
                {
                    Value = item.Value;
                }
            }
        }

        public async override Task OnNavigatedFromAsync(INavigatedFromParameters parameters)
        {
            await parameters.PageState.TrySetAsync(nameof(Value), Value);
        }

        public async override Task<bool> CanNavigateAsync(IConfirmNavigationParameters parameters)
        {
            var goingToDetails = parameters.ToNavigationInfo.PageType == typeof(Views.DetailPage);
            if (goingToDetails)
            {
                var dialog = new ContentDialog
                {
                    Title = "Confirmation",
                    Content = "Are you sure?",
                    PrimaryButtonText = "Continue",
                    SecondaryButtonText = "Cancel",
                };
                var result = await dialog.ShowAsyncEx();
                return result != ContentDialogResult.Secondary;
            }
            else
            {
                return true;
            }
        }

        public void GotoDetailsPage() => NavigationService.Navigate(typeof(Views.DetailPage), Value);

        public void GotoSettings() => NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() => NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() => NavigationService.Navigate(typeof(Views.SettingsPage), 2);
    }
}
