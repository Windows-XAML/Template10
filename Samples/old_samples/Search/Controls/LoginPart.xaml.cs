using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.SearchSample.Controls
{
    public sealed partial class LoginPart : UserControl
    {
        public LoginPart()
        {
            this.InitializeComponent();
        }

        public event EventHandler HideRequested;
        public event EventHandler LoggedIn;

        private void LoginClicked(object sender, RoutedEventArgs e)
        {
            LoggedIn?.Invoke(this, EventArgs.Empty);
        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            HideRequested?.Invoke(this, EventArgs.Empty);
        }

        public Models.UserCredentials UserCredentials { get; set; }
    }
}
