using System.Linq;
using Template10.Core;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Template10.Core
{
    public interface ITemplate10Window
    {
        Window Window { get; }
        ApplicationView ApplicationView { get; }
        bool IsMainView { get; }
        UIElement Content { get; set; }
        ITemplate10Dispatcher Dispatcher { get; }
        DisplayInformation DisplayInformation { get; }
        UIViewSettings UIViewSettings { get; }

        void Close();
    }
}