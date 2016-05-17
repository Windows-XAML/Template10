using System.ComponentModel;
using System.Linq;
using System;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Sample.ViewModels;

namespace Sample.Views
{
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;
        Services.SettingsServices.SettingsService _settings;

        public Shell()
        {
            Instance = this;
            InitializeComponent();
            _settings = Services.SettingsServices.SettingsService.Instance;
        }

        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }

        public void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
            HamburgerMenu.RefreshStyles(_settings.AppTheme, true);
            HamburgerMenu.IsFullScreen = _settings.IsFullScreen;
            HamburgerMenu.HamburgerButtonVisibility = _settings.ShowHamburgerButton ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SearchTextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var text = SearchTextBox.Text;
            var nav = BootStrapper.Current.NavigationService;
            if (nav.CurrentPageType == typeof(Views.SearchPage))
            {
                var page = nav.Content as SearchPage;
                var data = page.DataContext as SearchPageViewModel;
                data.Filter = text;
                data.ApplyFilter();
            }
            else
            {
                nav.Navigate(typeof(SearchPage), text);
            }
        }
    }
}
