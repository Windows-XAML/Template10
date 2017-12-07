using Windows.UI.Xaml;

namespace Template10.Common
{
    public partial class WindowEx : IWindowEx
    {
        IWindowEx Two => this as IWindowEx;

        Window IWindowEx2.Window { get; set; }
    }
}
