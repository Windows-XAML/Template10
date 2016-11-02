using System.Collections.Generic;

namespace Template10.Services.Lifetime
{
    public interface IViewService
    {
        bool IsInMainView { get; }
        Windows.UI.Xaml.Window Window { get; }
        Windows.Graphics.Display.DisplayInformation DisplayInformation { get; }
        Windows.UI.ViewManagement.ApplicationView ApplicationView { get; }
        Windows.UI.ViewManagement.UIViewSettings UIViewSettings { get; }
        List<Navigation.INavigationService> NavigationServices { get; }
    }

}