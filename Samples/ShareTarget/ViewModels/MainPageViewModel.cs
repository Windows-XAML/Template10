using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace ShareTarget.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        string _Date = default(string);
        public string Date { get { return _Date; } set { Set(ref _Date, value); } }

        public void Do()
        {
            Date = DateTime.Now.ToString();
        }
    }
}
