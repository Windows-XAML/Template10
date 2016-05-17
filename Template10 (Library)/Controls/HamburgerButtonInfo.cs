using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Controls
{
    [ContentProperty(Name = nameof(Content))]
    public class HamburgerButtonInfo : DependencyBindableBase
    {
        public enum ButtonTypes { Toggle, Command, Literal }

        public ButtonTypes ButtonType
        {
            get { return (ButtonTypes)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }
        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register(nameof(ButtonType), typeof(ButtonTypes),
                typeof(HamburgerButtonInfo), new PropertyMetadata(ButtonTypes.Toggle));

        public NavigationTransitionInfo NavigationTransitionInfo
        {
            get { return (NavigationTransitionInfo)GetValue(NavigationTransitionInfoProperty); }
            set { SetValue(NavigationTransitionInfoProperty, value); }
        }
        public static readonly DependencyProperty NavigationTransitionInfoProperty =
            DependencyProperty.Register(nameof(NavigationTransitionInfo), typeof(NavigationTransitionInfo),
                typeof(HamburgerButtonInfo), new PropertyMetadata(null));

        /// <summary>  
        /// Sets and gets the Command property.  
        /// </summary>  
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
             DependencyProperty.Register(nameof(Command), typeof(ICommand),
             typeof(HamburgerButtonInfo), new PropertyMetadata(null));

        public object ToolTip
        {
            get { return (object)GetValue(ToolTipProperty); }
            set { SetValue(ToolTipProperty, value); }
        }
        public static readonly DependencyProperty ToolTipProperty =
            DependencyProperty.Register(nameof(ToolTip), typeof(object), 
                typeof(HamburgerButtonInfo), new PropertyMetadata(null));

        /// <summary>  
        /// Sets and gets the CommandParameter property.  
        /// </summary>  
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
          DependencyProperty.Register(nameof(CommandParameter), typeof(object),
          typeof(HamburgerButtonInfo), new PropertyMetadata(null));

        /// <summary>
        /// Sets and gets the PageType property.
        /// </summary>
        public Type PageType
        {
            get { return (Type)GetValue(PageTypeProperty); }
            set { SetValue(PageTypeProperty, value); }
        }
        public static readonly DependencyProperty PageTypeProperty =
            DependencyProperty.Register(nameof(PageType), typeof(Type),
                typeof(HamburgerButtonInfo), new PropertyMetadata(null));

        /// <summary>
        /// Sets and gets the PageParameter property.
        /// </summary>
        public object PageParameter
        {
            get { return GetValue(PageParameterProperty); }
            set { SetValue(PageParameterProperty, value); }
        }
        public static readonly DependencyProperty PageParameterProperty =
            DependencyProperty.Register(nameof(PageParameter), typeof(object),
                typeof(HamburgerButtonInfo), new PropertyMetadata(null));

        /// <summary>
        /// Sets and gets the ClearHistory property.
        /// If true, navigation stack is cleared when navigating to this page
        /// </summary>
        public bool ClearHistory
        {
            get { return (bool)GetValue(ClearHistoryProperty); }
            set { SetValue(ClearHistoryProperty, value); }
        }
        public static readonly DependencyProperty ClearHistoryProperty =
            DependencyProperty.Register(nameof(ClearHistory), typeof(bool),
                typeof(HamburgerButtonInfo), new PropertyMetadata(false));

        /// <summary>
        /// Sets and gets the ClearCache property.
        /// If true, navigation page cache is cleared when navigating to this page
        /// </summary>
        public bool ClearCache
        {
            get { return (bool)GetValue(ClearCacheProperty); }
            set { SetValue(ClearCacheProperty, value); }
        }
        public static readonly DependencyProperty ClearCacheProperty =
            DependencyProperty.Register(nameof(ClearCache), typeof(bool),
                typeof(HamburgerButtonInfo), new PropertyMetadata(false));

        /// <summary>
        /// Sets and gets the Visibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }
        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register(nameof(Visibility), typeof(Visibility),
                typeof(HamburgerButtonInfo), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Sets and gets the IsEnabled property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register(nameof(IsEnabled), typeof(bool),
                typeof(HamburgerButtonInfo), new PropertyMetadata(true));

        public bool? IsChecked
        {
            get { return (bool?)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(object),
                typeof(HamburgerButtonInfo), new PropertyMetadata(false));

        public UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(UIElement),
                typeof(HamburgerButtonInfo), new PropertyMetadata(null));

        public double MaxWidth
        {
            get { return (double)GetValue(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }
        public static readonly DependencyProperty MaxWidthProperty =
            DependencyProperty.Register(nameof(MaxWidth), typeof(double),
                typeof(HamburgerButtonInfo), new PropertyMetadata(9999d));

        public override string ToString() => string.Format($"IsChecked: {IsChecked} PageType: {PageType}, Parameter: {PageParameter}");

        #region Events

        public event RoutedEventHandler Selected;
        internal void RaiseSelected() => Selected?.Invoke(this, new RoutedEventArgs());

        public event RoutedEventHandler Unselected;
        internal void RaiseUnselected() => Unselected?.Invoke(this, new RoutedEventArgs());

        public event RoutedEventHandler Checked;
        internal void RaiseChecked(RoutedEventArgs args)
        {
            if (ButtonType == ButtonTypes.Toggle)
                Checked?.Invoke(this, args);
        }

        public event RoutedEventHandler Unchecked;
        internal void RaiseUnchecked(RoutedEventArgs args)
        {
            if (ButtonType == ButtonTypes.Toggle)
                Unchecked?.Invoke(this, args);
        }

        public event RoutedEventHandler Tapped;
        internal void RaiseTapped(RoutedEventArgs args) => Tapped?.Invoke(this, args);

        public event RightTappedEventHandler RightTapped;
        internal void RaiseRightTapped(Windows.UI.Xaml.Input.RightTappedRoutedEventArgs args) => RightTapped?.Invoke(this, args);

        public event HoldingEventHandler Holding;
        internal void RaiseHolding(HoldingRoutedEventArgs args) => Holding?.Invoke(this, args);

        #endregion
    }
}
