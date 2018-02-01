using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Prism.Windows.Controls
{
    public partial class NavViewProps : DependencyObject
    {
        // NavigationViewItem custom NavigationUri property

        public static string GetNavigationUri(DependencyObject obj)
            => (string)obj.GetValue(NavigationUriProperty);
        public static void SetNavigationUri(DependencyObject obj, string value)
            => obj.SetValue(NavigationUriProperty, value);
        public static readonly DependencyProperty NavigationUriProperty =
            DependencyProperty.RegisterAttached("NavigationUri", typeof(string),
                typeof(NavigationView), new PropertyMetadata(null));

        // NavigationViewItem custom IsStartPage property

        public static bool GetIsStartPage(NavigationViewItem obj)
            => (bool)obj.GetValue(IsStartPageProperty);
        public static void SetIsStartPage(NavigationViewItem obj, bool value)
            => obj.SetValue(IsStartPageProperty, value);
        public static readonly DependencyProperty IsStartPageProperty =
            DependencyProperty.RegisterAttached("IsStartPage", typeof(bool),
                typeof(NavViewProps), new PropertyMetadata(false));

        // Page custom HeaderText property

        public static string GetHeaderText(Page obj)
            => (string)obj.GetValue(HeaderTextProperty);
        public static void SetHeaderText(Page obj, string value)
            => obj.SetValue(HeaderTextProperty, value);
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.RegisterAttached("HeaderText", typeof(string),
                typeof(NavViewProps), new PropertyMetadata(null));

        // Page custom HeaderText property

        public static IEnumerable<ICommandBarElement> GetHeaderCommands(Page obj)
            => (IEnumerable<ICommandBarElement>)obj.GetValue(HeaderCommandsProperty);
        public static void SetHeaderCommands(Page obj, IEnumerable<ICommandBarElement> value)
            => obj.SetValue(HeaderCommandsProperty, value);
        public static readonly DependencyProperty HeaderCommandsProperty =
            DependencyProperty.RegisterAttached("HeaderCommands",
                typeof(IEnumerable<ICommandBarElement>), typeof(NavViewProps), new PropertyMetadata(null));
    }
}
