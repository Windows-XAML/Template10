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
            VisualStateManager.GoToState(this, NormalVisualState.Name, false);
        }

        public static void SetBusyVisibility(Visibility visible, string text = null)
        {
            Window.Dispatcher.Dispatch(() =>
            {
                switch (visible)
                {
                    case Visibility.Visible:
                        Instance.BusyText.Text = text ?? string.Empty;
                        VisualStateManager.GoToState(Instance, Instance.BusyVisualState.Name, true);
                        break;
                    default:
                        VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true);
                        break;
                }
            });
        }

        public static void SetLoginVisibility(Visibility visible)
        {
            Window.Dispatcher.Dispatch(() =>
            {
                switch (visible)
                {
                    case Visibility.Visible:
                        VisualStateManager.GoToState(Instance, Instance.LoginVisualState.Name, true);
                        break;
                    default:
                        VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true);
                        break;
                }
            });
        }
    }
}
