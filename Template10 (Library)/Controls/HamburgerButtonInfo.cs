using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public enum ButtonTypes { Toggle, Command }
        public ButtonTypes ButtonType { get; set; } = ButtonTypes.Toggle;

        private NavigationTransitionInfo _navigationTransitionInfo;
        public NavigationTransitionInfo NavigationTransitionInfo
        {
            get { return _navigationTransitionInfo; }
            set { _navigationTransitionInfo = value; }
        }

        /// <summary>
        /// Sets and gets the PageType property.
        /// </summary>
        Type _PageType;
        public Type PageType
        {
            get { return _PageType; }
            set { Set(ref _PageType, value); }
        }

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
        bool _clearHistory = false;
        public bool ClearHistory
        {
            get { return _clearHistory; }
            set { Set(ref _clearHistory, value); }
        }

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

        bool _isChecked = false;
        public bool? IsChecked
        {
            get { return _isChecked; }
            set { Set(ref _isChecked, value ?? false); }
        }

        UIElement _content = null;
        public UIElement Content
        {
            get { return _content; }
            set { Set(ref _content, value); }
        }

        double _maxWidth = 9999;
        public double MaxWidth
        {
            get { return _maxWidth; }
            set { Set(ref _maxWidth, value); }
        }

        public override string ToString() =>
            string.Format($"IsChecked: {IsChecked} PageType: {PageType}, Parameter: {PageParameter}");

        public event RoutedEventHandler Selected;
        internal void RaiseSelected()
        {
            Selected?.Invoke(this, new RoutedEventArgs());
        }

        public event RoutedEventHandler Unselected;
        internal void RaiseUnselected()
        {
            Unselected?.Invoke(this, new RoutedEventArgs());
        }

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
        internal void RaiseTapped(RoutedEventArgs args)
        {
            Tapped?.Invoke(this, args);
        }

        public event RightTappedEventHandler RightTapped;
        internal void RaiseRightTapped(Windows.UI.Xaml.Input.RightTappedRoutedEventArgs args)
        {
            RightTapped?.Invoke(this, args);
        }

        public event HoldingEventHandler Holding;
        internal void RaiseHolding(HoldingRoutedEventArgs args)
        {
            Holding?.Invoke(this, args);
        }
    }
}
