using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.ConvertersSample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        ViewModels.MainPageViewModel ViewModel => this.DataContext as ViewModels.MainPageViewModel;

        private void DisplayText_Click(object sender, RoutedEventArgs e)
        {
            ValueWhenConverterText.Visibility = Visibility.Visible;
        }

        private void SetFirstComboItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedSimpleModel = ViewModel.SimpleModels.FirstOrDefault();
        }

        private void SetNullValueToCombo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedSimpleModel = null;
        }
    }
}
