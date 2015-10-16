using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel.Resources;

namespace Sample.ViewModels
{
    public class BabPageViewModel : Sample.Mvvm.ViewModelBase
    {

        public BabPageViewModel()
        {
   
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Views.Shell.HamburgerMenu.PageHasBottomAppBar = true;
        }

        public override void OnNavigatingFrom(NavigatingEventArgs args)
        {
            Views.Shell.HamburgerMenu.PageHasBottomAppBar = false;
        }

    }
}
