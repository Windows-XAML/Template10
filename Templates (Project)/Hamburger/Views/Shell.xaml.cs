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

        public UserService User => new UserService();
    }

    public class UserService : BindableBase
    {
        public UserService()
        {
            StartAsync();
        }

        async void StartAsync()
        {
            try
            {
                var users = await Windows.System.User.FindAllAsync();
                var current = users.FirstOrDefault();
                FirstName = await current.GetPropertyAsync(Windows.System.KnownUserProperties.FirstName) as string;
                LastName = await current.GetPropertyAsync(Windows.System.KnownUserProperties.LastName) as string;
                DisplayName = $"{FirstName} {LastName}";
            }
            catch (Exception)
            {
                throw;
            }
        }

        string _FirstName = "None";
        public string FirstName { get { return _FirstName; } set { Set(ref _FirstName, value); } }

        string _LastName = "None";
        public string LastName { get { return _LastName; } set { Set(ref _LastName, value); } }

        string _DisplayName = "None";
        public string DisplayName { get { return _DisplayName; } set { Set(ref _DisplayName, value); } }
    }
}
