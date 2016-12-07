using Template10.Services.NavigationService;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Template10.Common
{
    public interface IWindowWrapper
    {
        object Content { get; }
        DispatcherWrapper Dispatcher { get; }
        NavigationServiceList NavigationServices { get; }
        Window Window { get; }

        ApplicationView ApplicationView();
        void Close();
        DisplayInformation DisplayInformation();
        UIViewSettings UIViewSettings();
    }
}