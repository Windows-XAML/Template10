using System.ComponentModel;
using System.Linq;
using System;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Mvvm;

namespace Template10.Samples.MasterDetailSample.Views
{
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;

        public Shell()
        {
            Instance = this;
            InitializeComponent();
        }

        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }

        public void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
        }

        private void Accounts_Tapped(object sender, RoutedEventArgs e)
        {
            Account1Button.Visibility = Account2Button.Visibility =
                (Account1Button.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Folders_Tapped(object sender, RoutedEventArgs e)
        {
            b1.Visibility = b2.Visibility = b3.Visibility = b4.Visibility = b5.Visibility = b6.Visibility = b7.Visibility =
                (b1.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
