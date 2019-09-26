using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Controls
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

        public static string GetHeaderText(FrameworkElement obj)
            => (string)obj.GetValue(HeaderTextProperty);
        public static void SetHeaderText(FrameworkElement obj, string value)
            => obj.SetValue(HeaderTextProperty, value);
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.RegisterAttached("HeaderText", typeof(string),
                typeof(NavViewProps), new PropertyMetadata(null));

        // shell CommandBar dyanmic item

        internal static bool GetPageHeaderCommandDynamicItem(DependencyObject obj)
            => (bool)obj.GetValue(PageHeaderCommandDynamicItemProperty);
        internal static void SetPageHeaderCommandDynamicItem(DependencyObject obj, bool value)
            => obj.SetValue(PageHeaderCommandDynamicItemProperty, value);
#pragma warning disable IDE1006 // Naming Styles
        internal static readonly DependencyProperty PageHeaderCommandDynamicItemProperty =
#pragma warning disable IDE1006 // Naming Styles
            DependencyProperty.RegisterAttached("PageHeaderCommandDynamicItem", typeof(bool), 
                typeof(NavViewProps), new PropertyMetadata(false));

        // Page custom HeaderText property

        public static ObservableCollection<object> GetHeaderCommands(Page obj)
        {
            var value = (ObservableCollection<object>)obj.GetValue(HeaderCommandsProperty);
            if (value == null)
            {
                SetHeaderCommands(obj, value = new ObservableCollection<object>());
                value.CollectionChanged += (s, e) =>
                {
                    // TODO
                };
            }
            return value;
        }
        public static void SetHeaderCommands(Page obj, ObservableCollection<object> value)
            => obj.SetValue(HeaderCommandsProperty, value);
        public static readonly DependencyProperty HeaderCommandsProperty =
            DependencyProperty.RegisterAttached("HeaderCommands",
                typeof(ObservableCollection<object>),
                typeof(NavViewProps), new PropertyMetadata(null));
    }
}
