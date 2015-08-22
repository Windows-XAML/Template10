using SecondaryTileActivation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace SecondaryTileActivation.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private string _greeting;

        public string Greeting
        {
            get { return _greeting; }
            set { Set(ref _greeting, value); }
        }

        public MainPageViewModel()
        {
            Greeting = "Hello .NET user Group";
        }

        public void GoToPage2()
        {
            NavigationService.Navigate(typeof(SecondPage), "A navigation parameter");
        }
    }
}
