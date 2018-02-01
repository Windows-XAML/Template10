
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Prism.Windows.Services.DialogService
{
    public static class DialogExtensions
    {
        public static ContentDialog SetOkToClose(this ContentDialog dialog, IDialogResourceResolver resolver = null)
        {
            resolver = resolver ?? Settings.DefaultResolver;
            dialog.PrimaryButtonText = resolver.Resolve(ResourceTypes.Ok);
            dialog.IsPrimaryButtonEnabled = true;
            return dialog;
        }

        public static ContentDialog SetPrimaryButton(this ContentDialog dialog, string text)
        {
            dialog.PrimaryButtonText = text;
            dialog.IsPrimaryButtonEnabled = true;
            return dialog;
        }

        public static ContentDialog SetCloseButton(this ContentDialog dialog, string text)
        {
            dialog.CloseButtonText = text;
            return dialog;
        }

        public static ContentDialog SetPrimaryButton(this ContentDialog dialog, string text,
            TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> clickHandler)
        {
            dialog.SetPrimaryButton(text);
            dialog.PrimaryButtonClick += clickHandler;
            return dialog;
        }

        public static ContentDialog SetSecondaryButton(this ContentDialog dialog, string text)
        {
            dialog.SecondaryButtonText = text;
            dialog.IsSecondaryButtonEnabled = true;
            return dialog;
        }

        public static ContentDialog SetSecondaryButton(this ContentDialog dialog, string text,
            TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> clickHandler)
        {
            dialog.SetSecondaryButton(text);
            dialog.SecondaryButtonClick += clickHandler;
            return dialog;
        }
    }
}
