using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Extensions;
using Windows.UI.Xaml.Controls;
using Template10.Navigation;
using Template10.Services.Dialog;
using Template10.Services.Logging;
using Template10.Services.Resources;
using Sample.Services;

namespace Sample.ViewModels
{
    public class MainPageViewModel_Designtime : MainPageViewModel
    {
        public MainPageViewModel_Designtime() : base(null, null) { }
    }

    public class MainPageViewModel : ViewModelBase
    {
        ILoggingService _loggingService;
        ILocalDialogService _dialogService;

        public MainPageViewModel(ILocalDialogService dialog, ILoggingService logger)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // design-time
            }
            else
            {
                _loggingService = logger;
                _dialogService = dialog;
            }
        }

        static string _Value = "Gas";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public async override Task OnNavigatedToAsync(INavigatedToParameters parameter)
        {
            if (parameter.NavigationMode == NavMode.Back | parameter.Resuming)
            {
                await RestoreDataAsync(parameter);
            }
        }

        public async override Task OnNavigatedFromAsync(INavigatedFromParameters parameters)
        {
            await SaveDataAsync(parameters);
        }

        private async Task RestoreDataAsync(INavigatedToParameters parameter)
        {
            var item = await parameter.ToNavigationInfo.PageState.TryGetAsync<string>(nameof(Value));
            if (item.Success)
            {
                Value = item.Value;
            }
        }

        private async Task SaveDataAsync(INavigatedFromParameters parameters)
        {
            await parameters.PageState.TrySetAsync(nameof(Value), Value);
        }


        public async override Task<bool> CanNavigateAsync(IConfirmNavigationParameters parameters)
        {
            if (parameters.GoingTo(typeof(Views.DetailPage)))
            {
                return await _dialogService.ShowAreYouSureAsync();
            }
            else
            {
                return true;
            }
        }

        public void GotoDetailsPage()
                => NavigationService.Navigate(typeof(Views.DetailPage), Value);

        public void GotoSettings()
            => NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy()
            => NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout()
            => NavigationService.Navigate(typeof(Views.SettingsPage), 2);
    }
}
