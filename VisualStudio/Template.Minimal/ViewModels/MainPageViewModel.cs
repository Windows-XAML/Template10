using System.Threading.Tasks;
using Template10.Extensions;
using Template10.Mvvm;
using Template10.Navigation;
using Template10.Services.Dialog;
using Windows.UI.Xaml.Controls;

namespace Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        IDialogService _DialogService;

        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Value = "Designtime value";
            }
            else
            {
                _DialogService = new DialogService();
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
            var result = await _DialogService.PromptAsync("Are you sure?");
            return result == MessageBoxResult.Yes;
        }

        public void GotoDetailsPage() => NavigationService.Navigate(typeof(Views.DetailPage), Value);

        public void GotoSettings() => NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() => NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() => NavigationService.Navigate(typeof(Views.SettingsPage), 2);
    }
}
