using System;
using System.ComponentModel;
using System.Linq;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Sample.Services.SettingsServices;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;

namespace Sample.Views
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }

        public Shell(NavigationService navigationService)
        {
            Instance = this;
            InitializeComponent();
            BootStrapper.Current.HamburgerMenu = Instance.MyHamburgerMenu;
            MyHamburgerMenu.NavigationService = navigationService;
            VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true);
            SetRequestedTheme(SettingsService.Instance.AppTheme);
        }

        public static void SetBusyVisibility(Visibility visible, string text = null)
        {
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                switch (visible)
                {
                    case Visibility.Visible:
                        Instance.FindName(nameof(BusyScreen));
                        Instance.BusyText.Text = text ?? string.Empty;
                        if (VisualStateManager.GoToState(Instance, Instance.BusyVisualState.Name, true))
                        {
                            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                                AppViewBackButtonVisibility.Collapsed;
                        }
                        break;
                    case Visibility.Collapsed:
                        if (VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true))
                        {
                            BootStrapper.Current.UpdateShellBackButton();
                        }
                        break;
                }
            });
        }

        public static void SetRequestedTheme(ApplicationTheme theme)
        {
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                switch (theme)
                {
                    case ApplicationTheme.Light:
                        Instance.RequestedTheme = ElementTheme.Light;
                        break;
                    case ApplicationTheme.Dark:
                        Instance.RequestedTheme = ElementTheme.Dark;
                        break;
                    default:
                        Instance.RequestedTheme = ElementTheme.Default;
                        break;
                }
                BootStrapper.Current.NavigationService.Refresh();
            });
        }
    }
}
