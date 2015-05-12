using Blank1.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Blank1.ViewModels
{
    class Page2ViewModel : ViewModelBase
    {
        public Page2ViewModel()
        {
            this.MyParameter = "Set from the constructor";
        }

        public override void OnNavigatedTo(string parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            this.MyParameter = parameter?.ToString() ?? "Empty";
        }

        private string _MyParameter = default(string);
        public string MyParameter { get { return _MyParameter; } set { Set(ref _MyParameter, value); } }
    }
}
