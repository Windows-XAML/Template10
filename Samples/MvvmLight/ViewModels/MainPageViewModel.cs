using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Sample.ViewModels
{
    public class MainPageViewModel : Mvvm.ViewModelBase
    {
        public MainPageViewModel()
        {
            if (base.IsInDesignMode)
                Value = "Designtime value";
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Value = parameter?.ToString();
        }

        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { Set(ref _Value, value); }
        }
    }
}
