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
            navigationService.FrameFacade.Navigated += (s, e) => VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, false);
            VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, false);
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
                        VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true);
                        break;
                }
            });
        }

        private void LoginTapped(object sender, RoutedEventArgs e)
        {
            (sender as HamburgerButtonInfo).IsChecked = false;
            VisualStateManager.GoToState(this, LoginVisualState.Name, true);
        }

        private void LoginHide(object sender, System.EventArgs e)
        {
            VisualStateManager.GoToState(this, NormalVisualState.Name, true);
            // only for demonstration purposes
            // the user would need to really login in the real world
            IsAuthenticated = true;
        }

        private void SearchChecked(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, SearchVisualState.Name, true);
        }

        private void SearchUnchecked(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, NormalVisualState.Name, true);
        }

        public bool IsAuthenticated
        {
            get { return (bool)GetValue(IsAuthenticatedProperty); }
            set { SetValue(IsAuthenticatedProperty, value); }
        }
        public static readonly DependencyProperty IsAuthenticatedProperty =
            DependencyProperty.Register(nameof(IsAuthenticated), typeof(bool),
                typeof(Shell), new PropertyMetadata(false));
    }
}
