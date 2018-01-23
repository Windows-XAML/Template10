using Template10.Navigation;
using Windows.UI.Xaml.Controls;

namespace PrismSample.Views
{
    public sealed partial class ShellPage : Page
    {
        private NavigationService _navigationService;

        public ShellPage(out NavigationService service)
        {
            InitializeComponent();
            _navigationService = service = new NavigationService(ContentFrame);
        }
    }
}
