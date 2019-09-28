using Windows.UI.Xaml;

namespace Template10.Services
{
    public interface IMessageBoxEx
    {
        IDialogResourceResolver Resolver { get; set; }
        MessageBoxType Type { get; set; }
        string Text { get; set; }
        ElementTheme RequestedTheme { get; set; }
    }
}
