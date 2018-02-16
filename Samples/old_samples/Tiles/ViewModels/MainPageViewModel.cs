using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;

namespace Template10.Samples.TilesSample.ViewModels
{
    public class MainPageViewModel : Template10.Mvvm.ViewModelBase
    {
        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime data
                Value = "Designtime value";
                return;
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                // use cache value(s)
                if (state.ContainsKey(nameof(Value))) Value = state[nameof(Value)]?.ToString();
                // clear any cache
                state.Clear();
            }
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                // persist into cache
                state[nameof(Value)] = Value;
            }
            return base.OnNavigatedFromAsync(state, suspending);
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            return Task.CompletedTask;
        }

        private string _Value = string.Empty;
        public string Value
        {
            get { return _Value; }
            set
            {
                if (Set(ref _Value, value))
                    GotoDetailsPageCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand gotoDetailsPageCommand;
        public DelegateCommand GotoDetailsPageCommand => gotoDetailsPageCommand ?? (gotoDetailsPageCommand = new DelegateCommand(() =>
                                                                       {
                                                                           NavigationService.Navigate(typeof(Views.DetailPage), Value);
                                                                       },
    () => !string.IsNullOrEmpty(_Value)));
    }
}
