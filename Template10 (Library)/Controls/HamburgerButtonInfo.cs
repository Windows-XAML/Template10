﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Controls
{
    [ContentProperty(Name = nameof(Content))]
    public class HamburgerButtonInfo : DependencyBindableBase
    {
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
        object _PageParameter;
        public object PageParameter
        {
            get { return _PageParameter; }
            set { Set(ref _PageParameter, value); }
        }

        /// <summary>
        /// Sets and gets the ClearHistory property.
        /// If true, navigation stack is cleared when navigating to this page
        /// </summary>
        bool _ClearHistory = false;
        public bool ClearHistory
        {
            get { return _ClearHistory; }
            set { Set(ref _ClearHistory, value); }
        }

        /// <summary>
        /// Sets and gets the Visibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        Visibility _visibility = Visibility.Visible;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { Set(ref _visibility, value); }
        }

        /// <summary>
        /// Sets and gets the IsEnabled property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { Set(ref _isEnabled, value); }
        }

        bool _isChecked = false;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { Set(ref _isChecked, value); }
        }

        UIElement _content = null;
        public UIElement Content
        {
            get { return _content; }
            set { Set(ref _content, value); }
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", PageType?.ToString() ?? "null", PageParameter?.ToString() ?? "null");
        }

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
            Checked?.Invoke(this, args);
        }

        public event RoutedEventHandler Unchecked;
        internal void RaiseUnchecked(RoutedEventArgs args)
        {
            Unchecked?.Invoke(this, args);
        }

        public event RoutedEventHandler Tapped;
        internal void RaiseTapped(RoutedEventArgs args)
        {
            Tapped?.Invoke(this, args);
        }
    }
}
