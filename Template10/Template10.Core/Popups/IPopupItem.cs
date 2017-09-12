using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Popups
{
    public interface IPopupItem
    {
        void Initialize();
        DataTemplate Template { get; set; }
        TransitionCollection TransitionCollection { get; set; }
        Brush BackgroundBrush { get; set; }
        bool IsShowing { get; set; }
    }
}