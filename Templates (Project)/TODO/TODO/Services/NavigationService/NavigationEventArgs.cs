using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public class NavigationEventArgs : EventArgs
    {
        public NavigationMode NavigationMode { get; set; }

        public string Parameter { get; set; }
    }
}
