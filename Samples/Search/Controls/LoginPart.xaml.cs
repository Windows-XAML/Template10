using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Controls
{
    public sealed partial class LoginPart : UserControl
    {
        public LoginPart()
        {
            this.InitializeComponent();
        }

        public event EventHandler Hide;

        private void LoginClicked(object sender, RoutedEventArgs e)
        {
            Hide?.Invoke(this, EventArgs.Empty);
        }
    }
}
