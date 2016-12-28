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
        /// Sets and gets the GroupName property.
        /// In simplest form, a NavButton acting as a parent menu handles its Tapped event with the remaining group 
        /// members as submenu children. This parent can then act upon its children, such as toggling their visibility.
        /// You can, no doubt, find other more advanced uses for this though haven't figured out one yet ...
        /// </summary>
        public object GroupName
        {
            get { return GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }
        public static readonly DependencyProperty GroupNameProperty =
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
                typeof(HamburgerButtonInfo), new PropertyMetadata(true,
                    (sender, args) => ((HamburgerButtonInfo) sender).UpdateInternalBindingValues()));

        public bool? IsChecked
        {
            get { return (bool?)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(object),
                typeof(HamburgerButtonInfo), new PropertyMetadata(false));

        public bool IsTabStop
        {
            get { return (bool)GetValue(IsTabStopProperty); }
            set { SetValue(IsTabStopProperty, value); }
        }
        public static readonly DependencyProperty IsTabStopProperty =
            DependencyProperty.Register(nameof(IsTabStop), typeof(object),
                typeof(HamburgerButtonInfo), new PropertyMetadata(true,
                    (sender, args) => ((HamburgerButtonInfo)sender).UpdateInternalBindingValues()));

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

        #region Internal binding properties

        private void UpdateInternalBindingValues()
        {
            bool isFullScreen = this.IsFullScreen;
            bool isEnabled = !isFullScreen && this.IsEnabled;
            bool isTabStop = isEnabled && this.IsTabStop;

            this.IsEnabled_InternalBinding = isEnabled;
            this.IsTabStop_InternalBinding = isTabStop;
        }

        internal bool IsFullScreen
        {
            get { return (bool)this.GetValue(IsFullScreenProperty); }
            set { this.SetValue(IsFullScreenProperty, value); }
        }
        internal static readonly DependencyProperty IsFullScreenProperty =
            DependencyProperty.Register(nameof(IsFullScreenProperty), typeof(object),
                typeof(HamburgerButtonInfo), new PropertyMetadata(false,
                    (sender, args) => ((HamburgerButtonInfo)sender).UpdateInternalBindingValues()));

        internal bool IsEnabled_InternalBinding
        {
            get { return (bool)this.GetValue(IsEnabled_InternalBindingProperty); }
            set { this.SetValue(IsEnabled_InternalBindingProperty, value); }
        }

        internal static readonly DependencyProperty IsEnabled_InternalBindingProperty =
            DependencyProperty.Register(nameof(IsEnabled_InternalBinding), typeof(object),
                typeof(HamburgerButtonInfo), new PropertyMetadata(true));

        internal bool IsTabStop_InternalBinding
        {
            get { return (bool)this.GetValue(IsTabStop_InternalBindingProperty); }
            set { this.SetValue(IsTabStop_InternalBindingProperty, value); }
        }
        internal static readonly DependencyProperty IsTabStop_InternalBindingProperty =
            DependencyProperty.Register(nameof(IsTabStop_InternalBinding), typeof(object),
                typeof(HamburgerButtonInfo), new PropertyMetadata(true));

        #endregion

        #region Events

        public event RoutedEventHandler Selected;
        internal void RaiseSelected()
        {
            if (!IsFullScreen)
                this.Selected?.Invoke(this, new RoutedEventArgs());
        }

        public event RoutedEventHandler Unselected;
        internal void RaiseUnselected()
        {
            if (!IsFullScreen)
                this.Unselected?.Invoke(this, new RoutedEventArgs());
        }

        public event RoutedEventHandler Checked;
        internal void RaiseChecked(RoutedEventArgs args)
        {
            if ((ButtonType == ButtonTypes.Toggle) && !IsFullScreen)
                Checked?.Invoke(this, args);
        }

        public event RoutedEventHandler Unchecked;
        internal void RaiseUnchecked(RoutedEventArgs args)
        {
            if ((ButtonType == ButtonTypes.Toggle) && !IsFullScreen)
                Unchecked?.Invoke(this, args);
        }

        public event RoutedEventHandler Tapped;
        internal void RaiseTapped(RoutedEventArgs args)
        {
            if (!IsFullScreen)
                this.Tapped?.Invoke(this, args);
        }

        public event RightTappedEventHandler RightTapped;
        internal void RaiseRightTapped(Windows.UI.Xaml.Input.RightTappedRoutedEventArgs args)
        {
            if (!IsFullScreen)
                this.RightTapped?.Invoke(this, args);
        }

        public event HoldingEventHandler Holding;
        internal void RaiseHolding(HoldingRoutedEventArgs args)
        {
            if (!IsFullScreen)
                this.Holding?.Invoke(this, args);
        }

        #endregion
    }
}
