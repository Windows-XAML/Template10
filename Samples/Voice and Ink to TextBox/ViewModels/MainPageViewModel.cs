using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Template10.Samples.VoiceAndInkSample.ViewModels
{
    public class MainPageViewModel : Mvvm.ViewModelBase
    {
        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                Value = "Designtime value";
        }

        string _Value = string.Empty;
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                if (state.ContainsKey(nameof(Value)))
                    Value = state[nameof(Value)]?.ToString();
                state.Clear();
            }
            else
            {
                Value = parameter?.ToString();
            }
            return Task.CompletedTask;
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

        public void GotoDetailsPage()
        {
            NavigationService.Navigate(typeof(Views.DetailPage), Value);
        }

        public void GotoPrivacy()
        {
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);
        }

        public void GotoAbout()
        {
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);
        }

    }
}

