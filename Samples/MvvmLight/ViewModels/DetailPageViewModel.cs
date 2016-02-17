using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Messaging.ViewModels
{
    public class DetailPageViewModel : Mvvm.ViewModelBase
    {
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            return Task.CompletedTask;
        }
    }
}
