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

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            Views.Shell.Instance.ToggleState(string.Empty);
        }
    }
}
