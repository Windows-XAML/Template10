using System;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Sample.Views
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
    public sealed partial class Shell : Page
    {
        private static Shell Instance { get; set; }
        private static Template10.Common.WindowWrapper Window { get; set; }

        public Shell(NavigationService navigationService)
        {
            Instance = this;
            this.InitializeComponent();
            Window = Template10.Common.WindowWrapper.Current();
            MyHamburgerMenu.NavigationService = navigationService;
        }

        public static void SetBusyIndicator(bool busy, string text = null)
        {
            Window.Dispatcher.Dispatch(() =>
            {
                Instance.BusyIndicator.Visibility = (busy)
               ? Visibility.Visible : Visibility.Collapsed;
                Instance.BusyRing.IsActive = busy;
                Instance.BusyText.Text = text ?? string.Empty;
            });
        }

        public static void NotifyErrorMessage(string strMessage)
        {
            NotifyUser(strMessage, NotifyType.ErrorMessage);
        }

        public static void NotifyStatusMessage(string strMessage)
        {
            NotifyUser(strMessage, NotifyType.StatusMessage);
        }

        private static void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    Instance.StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
                case NotifyType.ErrorMessage:
                    Instance.StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
            }
            Instance.StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            Instance.StatusBorder.Visibility = (Instance.StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            if (Instance.StatusBlock.Text != String.Empty)
            {
                //Instance.StatusBorder.Visibility = Visibility.Visible;
                Instance.StatusPanel.Visibility = Visibility.Visible;
            }
            else
            {
                //Instance.StatusBorder.Visibility = Visibility.Collapsed;
                Instance.StatusPanel.Visibility = Visibility.Collapsed;
            }
        }

        public enum NotifyType
        {
            StatusMessage,
            ErrorMessage
        };

        private void StatusBlock_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
           // Instance.StatusBorder.Visibility = Visibility.Collapsed;
            Instance.StatusPanel.Visibility = Visibility.Collapsed;
        }
    }
}

