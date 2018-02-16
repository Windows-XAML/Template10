using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.BottomAppBarSample.Views
{
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu { get { return Instance.MyHamburgerMenu; } }

        public Shell(INavigationService navigationService)
        {
            Instance = this;
            InitializeComponent();
            HamburgerMenu.NavigationService = navigationService;
        }
    }
}

