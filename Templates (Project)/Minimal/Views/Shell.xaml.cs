using System;
using System.ComponentModel;
using System.Linq;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            this.InitializeComponent();
            Window = Template10.Common.WindowWrapper.Current();
            MyHamburgerMenu.NavigationService = navigationService;
            navigationService.FrameFacade.Navigated += (s, e) => ToggleState(string.Empty);
            ToggleState(string.Empty);
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
                        break;
                    default:
                        Instance.ToggleState(string.Empty);
                        break;
                }
            });
        }

        private void SearchTapped(object sender, RoutedEventArgs e)
        {
            var info = sender as HamburgerButtonInfo;
            info.IsChecked = false;
            ToggleState(SearchVisualState.Name);
        }

        private void LoginTapped(object sender, RoutedEventArgs e)
        {
            var info = sender as HamburgerButtonInfo;
            info.IsChecked = false;
            ToggleState(LoginVisualState.Name);
        }

        public void ToggleState(string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                VisualStateManager.GoToState(this, NormalVisualState.Name, false);
            }
            else
            {
                var current = VisualStateGroup.CurrentState.Name;
                if (current.Equals(state))
                    state = NormalVisualState.Name;
                VisualStateManager.GoToState(this, state, true);
            }
        }
    }
}
