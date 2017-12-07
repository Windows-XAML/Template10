using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Controls
{
    public partial class NavProperties : DependencyObject
    {
        public static Type GetPageType(NavigationViewItem obj)
            => (Type)obj.GetValue(PageTypeProperty);
        public static void SetPageType(NavigationViewItem obj, Type value)
            => obj.SetValue(PageTypeProperty, value);
        public static readonly DependencyProperty PageTypeProperty =
            DependencyProperty.RegisterAttached("PageType", typeof(Type),
                typeof(NavProperties), new PropertyMetadata(null));

        public static bool GetIsStartPage(NavigationViewItem obj)
            => (bool)obj.GetValue(IsStartPageProperty);
        public static void SetIsStartPage(NavigationViewItem obj, bool value)
            => obj.SetValue(IsStartPageProperty, value);
        public static readonly DependencyProperty IsStartPageProperty =
            DependencyProperty.RegisterAttached("IsStartPage", typeof(bool),
                typeof(NavProperties), new PropertyMetadata(false));

        public static string GetHeader(Page obj)
            => (string)obj.GetValue(HeaderProperty);
        public static void SetHeader(Page obj, string value)
            => obj.SetValue(HeaderProperty, value);
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.RegisterAttached("Header", typeof(string),
                typeof(NavProperties), new PropertyMetadata(null));
    }
}
