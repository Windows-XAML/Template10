using Samples.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class ItemPage : Page
    {
        public ItemPage()
        {
            InitializeComponent();
        }

        ViewModels.ItemPageViewModel ViewModel => DataContext as ViewModels.ItemPageViewModel;

        private async void PopOpen_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoBack();
            var character = ViewModel.Member.Character;
            await NewWindowHelper.CreateWindowAsync(MainGrid, character);
        }
    }
}
