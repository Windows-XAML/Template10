using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using System.Threading;
using Template10.Common;
using Windows.UI.Core;
using System.Diagnostics;

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

        string _Value = "Gas";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {
                Value = suspensionState[nameof(Value)]?.ToString();
            }

            await Task.Delay(500);
            Debug.WriteLine($"OnNavigatedToAsync {Dispatcher.HasThreadAccess()}");

            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                suspensionState[nameof(Value)] = Value;
            }

            await Task.Delay(500);
            Debug.WriteLine($"OnNavigatedFromAsync {Dispatcher.HasThreadAccess()}");

            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            Debug.WriteLine($"OnNavigatingFromAsync {Dispatcher.HasThreadAccess()}");


            //var dialog = new ContentDialogEx
            //{
            //    Title = "Confirmation",
            //    Content = "Are you sure?",
            //    PrimaryButtonText = "Continue",
            //    SecondaryButtonText = "Cancel",
            //};
            //var result = await dialog.ShowAsync();
            //args.Cancel = result == ContentDialogResult.Secondary;
        }

        public void GotoDetailsPage() => NavigationService.Navigate(typeof(Views.DetailPage), Value);

        public void GotoSettings() => NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() => NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() => NavigationService.Navigate(typeof(Views.SettingsPage), 2);
    }

    public class ContentDialogEx : ContentDialog
    {
        private class BooleanEx { public bool Value { get; set; } }
        private readonly static BooleanEx _ShowingDialog = new BooleanEx { Value = false };
        public new async Task<ContentDialogResult> ShowAsync() => await ShowAsync(null);
        public async Task<ContentDialogResult> ShowAsync(IDispatcherWrapper dispatcher)
        {
            lock (_ShowingDialog)
            {
                while (_ShowingDialog.Value)
                {
                    Monitor.Wait(_ShowingDialog);
                }
                _ShowingDialog.Value = true;
            }
            dispatcher = dispatcher ?? DispatcherWrapper.Current();
            var result = await dispatcher.DispatchAsync(async () => await base.ShowAsync());
            lock (_ShowingDialog)
            {
                _ShowingDialog.Value = false;
                Monitor.PulseAll(_ShowingDialog);
            }
            return result;
        }
    }
}
