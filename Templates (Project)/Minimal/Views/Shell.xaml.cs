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

namespace Sample.Views
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        private static Template10.Common.WindowWrapper Window { get; set; }

        public Shell(NavigationService navigationService)
        {
            Instance = this;
            InitializeComponent();
            Window = WindowWrapper.Current();
            MyHamburgerMenu.NavigationService = navigationService;
            VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true);
        }

        public static void SetBusyVisibility(Visibility visible, string text = null)
        {
            Window.Dispatcher.Dispatch(() =>
            {
                switch (visible)
                {
                    case Visibility.Visible:
                        Instance.FindName(nameof(BusyScreen));
                        Instance.BusyText.Text = text ?? string.Empty;
                        VisualStateManager.GoToState(Instance, Instance.BusyVisualState.Name, true);
                        if (SettingsService.Instance.UseShellBackButton &&
                            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility ==
                            AppViewBackButtonVisibility.Visible)
                        {
                            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                                AppViewBackButtonVisibility.Collapsed;
                        }
                        break;
                    default:
                        VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true);
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                            (SettingsService.Instance.UseShellBackButton && Instance.MyHamburgerMenu.NavigationService.CanGoBack)
                                ? AppViewBackButtonVisibility.Visible
                                : AppViewBackButtonVisibility.Collapsed;
                        break;
                }
            });
        }
    }
}
