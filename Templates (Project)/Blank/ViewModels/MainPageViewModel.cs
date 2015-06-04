using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Template10.ViewModels
{
    public class MainPageViewModel : Mvvm.ViewModelBase
    {
        public MainPageViewModel()
        {
            // designtime data
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Value = "Designtime value";
            }
        }

        public override void OnNavigatedTo(string parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            // restore state
            if (state.ContainsKey("Value"))
            {
                Value = state["Value"]?.ToString();
            }
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            // save state
            if (suspending)
            {
                state["Value"] = Value;
            }
            return Task.FromResult<object>(null);
        }

        public void GotoPage2()
        {
            this.NavigationService.Navigate(typeof(Views.Page2), this.Value);
        }

        private string _Value = "Hello Template 10";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }
    }
}
