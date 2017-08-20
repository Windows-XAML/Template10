using System;
using System.Threading.Tasks;
using Template10.Services.Secrets;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Demo.SecretService
{
    public sealed partial class MainPage : Page
    {
        #region Fields

        private SecretHelper secretHelper;
        private MessageDialog messageDialog;

        #endregion

        #region Constructors
        public MainPage()
        {
            this.InitializeComponent();
            secretHelper = new SecretHelper();
            messageDialog = new MessageDialog("");
        }

        #endregion


        #region Event handlers
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }


        private async void PasswordOkButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

            if (!await IsPasswordValid(this.PwdEnterTextBox.Password.Trim(), this.PwdConfirmTextBox.Password.Trim()))
            {
                return;
            }
            else
            {
                secretHelper.WriteSecret("demo", this.PwdEnterTextBox.Password.Trim());
                messageDialog.Content = "Password is saved into OS's credentials vault.\nNow you can verify it with the step below.";
                this.PwdEnterTextBox.Password = string.Empty;
                this.PwdConfirmTextBox.Password = string.Empty;
                await messageDialog.ShowAsync();
            }
        }

        private async void PwdSignInOkButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

            if (this.PwdSignInTextBox.Password.Trim().Length < 8 || this.PwdSignInTextBox.Password.Trim().Length > 24)
            {
                messageDialog.Content = "Sorry, password match failed!\n\nFurthermore, password length must be between 8 and 24 characters.\nIn practical application I may not tell you this other than prompting invalid password.\n\nNote also that leading and trailing white spaces will be truncated.";
                await messageDialog.ShowAsync();
                return;
            }

            if (secretHelper.ReadSecret("demo") == string.Empty)
            {
                messageDialog.Content = messageDialog.Content = "Sorry, password match failed!\n\nIn fact, there is no stored password and in practical application I won't tell you this.";
                this.PwdSignInTextBox.Password = string.Empty;
            }
            else if (secretHelper.ReadSecret("demo") == this.PwdSignInTextBox.Password.Trim())
            {
                messageDialog.Content = "Password matched! Consider that you've signed in!";
                this.PwdSignInTextBox.Password = string.Empty;
            }
            else
            {
                messageDialog.Content = messageDialog.Content = "Sorry, password match failed!\n\nYou know already that, with this simplistic demo, you can reset your password in the step above ;-)";
            }

            await messageDialog.ShowAsync();
        }

        private async Task<bool> IsPasswordValid(string password, string confirm)
        {
            if (password.Length < 8 || password.Length > 24)
            {
                messageDialog.Content = "Password length must be between 8 and 24 characters.\nNote that leading and trailing white spaces will be truncated.";
                await messageDialog.ShowAsync();
                return false;
            }

            if (password != confirm)
            {
                messageDialog.Content = "Password and confirmation do not match.\nPlease correct and try again.";
                await messageDialog.ShowAsync();
                return false;
            }

            return true;
        }

        #endregion

    }
}
