using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Samples.VoiceAndInkSample.Common
{
    class VisualTreeHelperEx
    {
        public static UIElement FindRoot(UIElement obj, bool rootIsContentPresenter)
        {
            UIElement parent = VisualTreeHelper.GetParent(obj) as UIElement;

            if (parent == null)
                return obj;
            else if (rootIsContentPresenter && parent is ContentPresenter)
                return parent;
            else return FindRoot(parent, rootIsContentPresenter);
        }
    }
}
