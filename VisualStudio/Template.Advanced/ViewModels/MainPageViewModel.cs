using System;
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
        Template10.Services.Logging.ILoggingService _logger;
        Template10.Services.Dialog.IContentService _content;
        Template10.Services.Resources.IResourceService _resources;

        public MainPageViewModel(
            Template10.Services.Dialog.IContentService content,
            Template10.Services.Logging.ILoggingService logger,
            Template10.Services.Resources.IResourceService resources)
        {
            _logger = logger;
            _content = content;
            _resources = resources;
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
            if (parameters.ToNavigationInfo.PageType == typeof(Views.DetailPage))
            {
                return await PromptAreYouSureAsync();
            }
            else
            {
                return true;
            }
        }

        private async Task<bool> PromptAreYouSureAsync()
        {
            var result = await _content.ShowAsync(
                content: "Are you sure?",
                title: "Confirmation",
                primaryButton: new Template10.Services.Dialog.ContentButtonInfo("Continue"),
                secondaryButton: new Template10.Services.Dialog.ContentButtonInfo("Cancel"));
            return result == ContentDialogResult.Primary;
        }

        public void GotoDetailsPage() => NavigationService.Navigate(typeof(Views.DetailPage), Value);

        public void GotoSettings() => NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() => NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() => NavigationService.Navigate(typeof(Views.SettingsPage), 2);
    }
}
