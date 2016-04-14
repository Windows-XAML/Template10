using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Template10.Samples.FileActivationSample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        string _Content = default(string);
        public string Content { get { return _Content; } set { Set(ref _Content, value); } }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Content = parameter.ToString();
            return Task.CompletedTask;
        }
    }
}

