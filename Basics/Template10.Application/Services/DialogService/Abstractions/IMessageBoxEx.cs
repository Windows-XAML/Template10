using Windows.UI.Xaml;

namespace Prism.Windows.Services.DialogService
{
    public interface IMessageBoxEx
    {
        IDialogResourceResolver Resolver { get; set; }
        MessageBoxType Type { get; set; }
        string Text { get; set; }
        ElementTheme RequestedTheme { get; set; }
    }
}
