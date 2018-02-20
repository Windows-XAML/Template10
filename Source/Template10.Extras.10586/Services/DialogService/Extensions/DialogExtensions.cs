
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Dialog
{
    public static class DialogExtensions
    {
        internal static ContentDialog SetOkToClose(this ContentDialog dialog, IDialogResourceResolver resolver = null)
        {
            resolver = resolver ?? Settings.DefaultResolver;
            dialog.PrimaryButtonText = resolver.Resolve(ResourceTypes.Ok);
            dialog.IsPrimaryButtonEnabled = true;
            return dialog;
        }

        internal static ContentDialog SetPrimaryButton(this ContentDialog dialog, string text)
        {
            dialog.PrimaryButtonText = text;
            dialog.IsPrimaryButtonEnabled = true;
            return dialog;
        }

        internal static ContentDialog SetCloseButton(this ContentDialog dialog, string text)
        {
            dialog.CloseButtonText = text;
            return dialog;
        }

        internal static ContentDialog SetPrimaryButton(this ContentDialog dialog, string text,
            TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> clickHandler)
        {
            dialog.SetPrimaryButton(text);
            dialog.PrimaryButtonClick += clickHandler;
            return dialog;
        }

        internal static ContentDialog SetSecondaryButton(this ContentDialog dialog, string text)
        {
            dialog.SecondaryButtonText = text;
            dialog.IsSecondaryButtonEnabled = true;
            return dialog;
        }

        internal static ContentDialog SetSecondaryButton(this ContentDialog dialog, string text,
            TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> clickHandler)
        {
            dialog.SetSecondaryButton(text);
            dialog.SecondaryButtonClick += clickHandler;
            return dialog;
        }
    }
}
