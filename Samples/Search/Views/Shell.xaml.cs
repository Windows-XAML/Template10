using System.Linq;
using Template10.Controls;
using Template10.Services.NavigationService;
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

        public void ToggleState(string state)
        {
            var states = VisualStateGroup.States.Select(x => x.Name);
            if (states.Contains(state))
            {
                var current = VisualStateGroup.CurrentState.Name;
                if (current.Equals(state))
                    state = NormalVisualState.Name;
                VisualStateManager.GoToState(this, state, true);
            }
            else
            {
                VisualStateManager.GoToState(this, NormalVisualState.Name, false);
            }
        }
    }
}
