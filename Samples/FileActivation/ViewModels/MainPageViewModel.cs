using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace FileActivation.ViewModels
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

