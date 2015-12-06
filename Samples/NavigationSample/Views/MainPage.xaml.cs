using NavigationSample.ViewModels;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			InitializeComponent();
		}

		// strongly-typed view models enable x:bind
		public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;
	}
}
