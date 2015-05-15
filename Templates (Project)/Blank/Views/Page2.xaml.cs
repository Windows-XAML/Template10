using Template10.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Template10.Views
{
    public sealed partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            DataContextChanged += (s, e) => { ViewModel = DataContext as Page2ViewModel; };
        }

        // strongly-typed view models enable x:bind
        public Page2ViewModel ViewModel { get; set; }
    }
}
