using System.ComponentModel;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Messaging.Views
{
    public sealed partial class Shell : Page, INotifyPropertyChanged
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu { get { return Instance.MyHamburgerMenu; } }

        public Shell(INavigationService navigationService)
        {
            Instance = this;
            InitializeComponent();
            HamburgerMenu.NavigationService = navigationService;
        }

        public bool IsBusy { get; set; } = false;
        public string BusyText { get; set; } = "Please wait...";
        public event PropertyChangedEventHandler PropertyChanged;

        public static void SetBusy(bool busy, string text = null)
        {
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                if (busy)
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                else
                    BootStrapper.Current.UpdateShellBackButton();

                Instance.IsBusy = busy;
                Instance.BusyText = text;

                Instance.PropertyChanged?.Invoke(Instance, new PropertyChangedEventArgs(nameof(IsBusy)));
                Instance.PropertyChanged?.Invoke(Instance, new PropertyChangedEventArgs(nameof(BusyText)));
            });
        }
    }
}

