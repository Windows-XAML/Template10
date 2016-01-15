using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

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

        private string _Value = string.Empty;
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                Value = state[nameof(Value)]?.ToString();
                state.Clear();
            }
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state[nameof(Value)] = Value;
            }
            return Task.CompletedTask;
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            return Task.CompletedTask;
        }

        public async Task GotoDetailsPage() =>
            await NavigationService.NavigateAsync(typeof(Views.DetailPage), Value);

        public async Task GotoPrivacy() =>
            await NavigationService.NavigateAsync(typeof(Views.SettingsPage), 1);

        public async Task GotoAbout() =>
            await NavigationService.NavigateAsync(typeof(Views.SettingsPage), 2);
    }
}