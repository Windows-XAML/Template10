using Template10.Samples.PageHeaderSample.ViewModels;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.PageHeaderSample.Views
{
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			InitializeComponent();
			NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;

			ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(100, 100));
		}

		// strongly-typed view models enable x:bind
		public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ViewModel.SelectionCount = ListView.SelectedItems.Count;
		}

		private void DeleteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			ViewModel.DeleteSelectedItems(ListView.SelectedItems);
		}
	}
}
