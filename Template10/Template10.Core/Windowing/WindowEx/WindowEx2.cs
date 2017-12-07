using Windows.UI.Xaml;

namespace Template10.Common
{
    public partial class WindowEx : IWindowEx2
    {
        IWindowEx2 Two => this as IWindowEx2;

        Window IWindowEx2.Window { get; set; }
    }
}
