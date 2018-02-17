using Template10.Samples.IncrementalLoadingSample.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.IncrementalLoadingSample.Views
{
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            this.InitializeComponent();
        }

        public MainViewModel ViewModel
        {
            get
            {
                return this.DataContext as MainViewModel;
            }
        }
    }
}