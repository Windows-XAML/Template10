using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.BottomAppBarSample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            //var menu = Shell.HamburgerMenu;
            //menu.Visibility = Visibility.Visible;
        }

        void TogglePane()
        {
            // Shell.HamburgerMenu.IsOpen = !Shell.HamburgerMenu.IsOpen;
        }

        DelegateCommand<int> _SetPaneCommand;
        public DelegateCommand<int> SetPaneCommand
           => _SetPaneCommand ?? (_SetPaneCommand = new DelegateCommand<int>(SetPaneCommandExecute));
        void SetPaneCommandExecute(int param)
        {
            return;
            var menu = Shell.HamburgerMenu;
            menu.DisplayMode = (SplitViewDisplayMode)param;

            menu.VisualStateNarrowMinWidth = -1;
            menu.VisualStateNormalMinWidth = -1;
            menu.VisualStateWideMinWidth = -1;
            MyPageHeader.Margin = new Thickness(0);

            switch (menu.DisplayMode)
            {
                case SplitViewDisplayMode.Inline:
                    //menu.HamburgerButtonVisibility = Visibility.Collapsed;
                    menu.IsOpen = true;
                    break;
                case SplitViewDisplayMode.CompactInline:
                    //menu.HamburgerButtonVisibility = Visibility.Visible;
                    menu.IsOpen = true;
                    break;
                case SplitViewDisplayMode.Overlay:
                    //menu.HamburgerButtonVisibility = Visibility.Visible;
                    // MyPageHeader.Margin = new Thickness(48,0,0,0);
                    menu.IsOpen = false;
                    break;
                case SplitViewDisplayMode.CompactOverlay:
                    //menu.HamburgerButtonVisibility = Visibility.Visible;
                    menu.IsOpen = false;
                    break;
            }
        }

    }
}
