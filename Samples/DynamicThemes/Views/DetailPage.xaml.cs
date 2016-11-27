using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Sample.Views
{
    public sealed partial class DetailPage : Page
    {
        double normalMinPageWidth = (double)Application.Current.Resources["TitleMenuToggleMinWidth"];
        public DetailPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;

            Loaded += TestPage_Loaded;
            Unloaded += TestPage_Unloaded;
        }

        private void TestPage_Loaded(object sender, RoutedEventArgs e)
        {
            Shell.HamburgerMenu.PaneOpened += HamburgerMenu_PaneOpened;
            Shell.HamburgerMenu.PaneClosed += HamburgerMenu_PaneClosed;
            SetVisualState();
        }
        private void TestPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Shell.HamburgerMenu.PaneOpened -= HamburgerMenu_PaneOpened;
            Shell.HamburgerMenu.PaneClosed -= HamburgerMenu_PaneClosed;
        }

        private void HamburgerMenu_PaneClosed(object sender, EventArgs e) { SetVisualState(MenuPane.IsClosed); }
        private void HamburgerMenu_PaneOpened(object sender, EventArgs e) { SetVisualState(MenuPane.IsOpened); }

        private void  SetVisualState(MenuPane menuPane = MenuPane.IsClosed)
        {
            if (ViewModel.PageWidth < normalMinPageWidth && menuPane == MenuPane.IsClosed)
            {
                VisualStateManager.GoToState(this, ShowMenuOnly.Name, true);
            }
            else if(ViewModel.PageWidth < normalMinPageWidth && menuPane == MenuPane.IsOpened)
            {
                VisualStateManager.GoToState(this, ShowTitleOnly.Name, true);
            }
            else
            {
                // Let XAML VSM do the job.
                //VisualStateManager.GoToState(this, NormalMode.Name, true);
            }

        }

        enum MenuPane
        {
            IsClosed,
            IsOpened
        }

        private void PageTitleMenuIconsToggleVisualGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            ;
        }
    }
}
