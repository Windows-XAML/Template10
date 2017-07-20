using System.Linq;
using Template10.Common;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Template10.Services.WindowWrapper
{
    public interface IWindowWrapper
    {
        Window Window { get; }
        object Content { get; }

        bool IsMainView { get; }

        IDispatcherWrapper Dispatcher { get; }
        ApplicationView ApplicationView { get; }
        DisplayInformation DisplayInformation { get; }
        UIViewSettings UIViewSettings { get; }
        void Close();
    }
}