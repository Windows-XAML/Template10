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

namespace Sample.ViewModels
{
    public class MainPageViewModel_Designtime : MainPageViewModel
    {
        public MainPageViewModel_Designtime() : base(null, null, null) { }
    }

    public class MainPageViewModel : ViewModelBase
    {
        ILoggingService _loggingService;
        IDialogService _dialogService;
        IResourceService _resourceService;

        public MainPageViewModel(IDialogService dialog, ILoggingService logger, IResourceService resources)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // design-time
            }
            else
            {
                _loggingService = logger;
                _dialogService = dialog;
                _resourceService = resources;
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
            if (parameters.ToNavigationInfo.PageType != typeof(Views.DetailPage))
            {
                return true;
            }
            var prompt = _resourceService.TryGetLocalizedString("AreYouSure", out var value) ? value : "Are you sure?";
            var result = await _dialogService.PromptAsync(prompt);
            return result == MessageBoxResult.Yes;
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
