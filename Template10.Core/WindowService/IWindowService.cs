using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.WindowService
{

    public interface IWindowService
    {
        bool IsInMainView { get; }
        Windows.UI.Xaml.Window Window { get; }
        Windows.Graphics.Display.DisplayInformation DisplayInformation { get; }
        Windows.UI.ViewManagement.ApplicationView ApplicationView { get; }
        Windows.UI.ViewManagement.UIViewSettings UIViewSettings { get; }
    }

}