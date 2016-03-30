using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VoiceAndInk.Common
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
