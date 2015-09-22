using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Template10.Services.KeyboardService;
using Template10.Services.NavigationService;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-HamburgerMenu
    [ContentProperty(Name = nameof(PrimaryButtons))]
    public sealed partial class HamburgerMenu : UserControl, INotifyPropertyChanged
    {
        public HamburgerMenu()
        {
            this.InitializeComponent();
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // nothing
            }
            else
            {
                PrimaryButtons = new NavigationButtonInfoCollection(this);
                SecondaryButtons = new NavigationButtonInfoCollection(this);
                new KeyboardService().AfterWindowZGesture = () => { HamburgerCommand.Execute(null); };
            }
        }

        void UpdateButtons(NavigatedEventArgs e) { UpdateButtons(e.PageType); }
        void UpdateButtons() { UpdateButtons(NavigationService.Frame.Content?.GetType()); }
        void UpdateButtons(Type type)
        {
            var info = _navButtons.FirstOrDefault(x => x.Value.PageType.Equals(type));
            Selected = info.Value;
        }

        Mvvm.DelegateCommand _hamburgerCommand;
        internal Mvvm.DelegateCommand HamburgerCommand { get { return _hamburgerCommand ?? (_hamburgerCommand = new Mvvm.DelegateCommand(ExecuteHamburger)); } }
        void ExecuteHamburger() { IsOpen = !IsOpen; }

        Mvvm.DelegateCommand<NavigationButtonInfo> _navCommand;
        public Mvvm.DelegateCommand<NavigationButtonInfo> NavCommand { get { return _navCommand ?? (_navCommand = new Mvvm.DelegateCommand<NavigationButtonInfo>(ExecuteNav)); } }
        void ExecuteNav(NavigationButtonInfo commandInfo)
        {
            if (commandInfo == null)
                throw new NullReferenceException("CommandParameter is not set");
            try
            {
                Selected = commandInfo;
            }
            finally
            {
                if (commandInfo.ClearHistory)
                    NavigationService.ClearHistory();
            }
        }

        #region VisualStateValues

        public double VisualStateNarrowMinWidth
        {
            get { return VisualStateNarrowTrigger.MinWindowWidth; }
            set { SetValue(VisualStateNarrowMinWidthProperty, VisualStateNarrowTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateNarrowMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNarrowMinWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).VisualStateNarrowMinWidth = (double)e.NewValue; }));

        public double VisualStateNormalMinWidth
        {
            get { return VisualStateNormalTrigger.MinWindowWidth; }
            set { SetValue(VisualStateNormalMinWidthProperty, VisualStateNormalTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateNormalMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNormalMinWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).VisualStateNormalMinWidth = (double)e.NewValue; }));

        public double VisualStateWideMinWidth
        {
            get { return VisualStateWideTrigger.MinWindowWidth; }
            set { SetValue(VisualStateWideMinWidthProperty, VisualStateWideTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateWideMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateWideMinWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).VisualStateWideMinWidth = (double)e.NewValue; }));

        #endregion

        #region Style

        public SolidColorBrush HamburgerBackground
        {
            get { return (SolidColorBrush)HamburgerBackgroundBrush; }
            set
            {
                SetValue(HamburgerBackgroundProperty, HamburgerBackgroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HamburgerBackground)));
            }
        }
        public static readonly DependencyProperty HamburgerBackgroundProperty =
            DependencyProperty.Register(nameof(HamburgerBackground), typeof(SolidColorBrush),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).HamburgerBackground = (SolidColorBrush)e.NewValue; }));

        public SolidColorBrush HamburgerForeground
        {
            get { return HamburgerForegroundBrush; }
            set
            {
                SetValue(HamburgerForegroundProperty, HamburgerForegroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HamburgerForeground)));
            }
        }
        public static readonly DependencyProperty HamburgerForegroundProperty =
              DependencyProperty.Register(nameof(HamburgerForeground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).HamburgerForeground = (SolidColorBrush)e.NewValue; }));

        public SolidColorBrush NavAreaBackground
        {
            get { return NavAreaBackgroundBrush; }
            set
            {
                SetValue(NavAreaBackgroundProperty, NavAreaBackgroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HamburgerForeground)));
            }
        }
        public static readonly DependencyProperty NavAreaBackgroundProperty =
              DependencyProperty.Register(nameof(NavAreaBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).NavAreaBackground = (SolidColorBrush)e.NewValue; }));

        public SolidColorBrush NavButtonBackground
        {
            get { return (SolidColorBrush)NavButtonBackgroundBrush; }
            set
            {
                SetValue(NavButtonBackgroundProperty, NavButtonBackgroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavButtonBackground)));
            }
        }
        public static readonly DependencyProperty NavButtonBackgroundProperty =
            DependencyProperty.Register(nameof(NavButtonBackground), typeof(SolidColorBrush),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).NavButtonBackground = (SolidColorBrush)e.NewValue; }));

        public SolidColorBrush NavButtonForeground
        {
            get { return NavButtonForegroundBrush; }
            set
            {
                SetValue(NavButtonForegroundProperty, NavButtonForegroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HamburgerForeground)));
            }
        }
        public static readonly DependencyProperty NavButtonForegroundProperty =
              DependencyProperty.Register(nameof(NavButtonForeground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).NavButtonForeground = (SolidColorBrush)e.NewValue; }));

        public SolidColorBrush SecondarySeparator
        {
            get { return SecondaryBorderBrush; }
            set
            {
                SetValue(SecondarySeparatorProperty, SecondaryBorderBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HamburgerForeground)));
            }
        }
        public static readonly DependencyProperty SecondarySeparatorProperty =
              DependencyProperty.Register("SecondarySeparator", typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).SecondarySeparator = (SolidColorBrush)e.NewValue; }));

        #endregion

        #region added by dg2k for HamburgerMenu enhancement

        public SolidColorBrush NavButtonCheckedOverlayBackground
        {
            get { return NavButtonCheckedOverlayBackgroundBrush; }
            set
            {
                SetValue(NavButtonCheckedOverlayBackgroundProperty, NavButtonCheckedOverlayBackgroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavButtonCheckedOverlayBackground)));
            }
        }
        public static readonly DependencyProperty NavButtonCheckedOverlayBackgroundProperty =
              DependencyProperty.Register(nameof(NavButtonCheckedOverlayBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).NavButtonCheckedOverlayBackground = (SolidColorBrush)e.NewValue; }));


        public SolidColorBrush NavButtonHoverOverlayBackground
        {
            get { return NavButtonHoverOverlayBackgroundBrush; }
            set
            {
                SetValue(NavButtonHoverOverlayBackgroundProperty, NavButtonHoverOverlayBackgroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavButtonHoverOverlayBackground)));
            }
        }
        public static readonly DependencyProperty NavButtonHoverOverlayBackgroundProperty =
              DependencyProperty.Register(nameof(NavButtonHoverOverlayBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).NavButtonHoverOverlayBackground = (SolidColorBrush)e.NewValue; }));

        #endregion

        public NavigationButtonInfo Selected
        {
            get
            {
                // do nothing if doesn't exist
                if (!_navButtons.Any(x => x.Key.IsChecked.Value))
                {
                    if (GetValue(SelectedProperty) != null)
                        SetValue(SelectedProperty, null);
                    return null;
                }

                // get value && ensure dp is also correct
                var value = _navButtons.First(x => x.Key.IsChecked.Value).Value;
                if (GetValue(SelectedProperty) != value)
                    SetValue(SelectedProperty, value);
                return value;
            }
            set
            {

                if (UpdateNavButtons(value))
                {
                    IsOpen = false;

                    // setup new value
                    var navButton = _navButtons.First(x => x.Value.Equals(value));
                    navButton.Key.IsChecked = true;
                    navButton.Key.IsEnabled = false;

                    // ensure dp is correct (if diff)
                    if (GetValue(SelectedProperty) != value)
                        SetValue(SelectedProperty, value);

                    // navigate only to new pages
                    if (value.PageType != null && NavigationService.CurrentPageType != value.PageType)
                    {
                        NavigationService.Navigate(value.PageType, value.PageParameter);
                    }
                }
            }
        }

        internal bool UpdateNavButtons(NavigationButtonInfo selected)
        {
            // clear existing
            foreach (var button in _navButtons)
            {
                button.Key.IsChecked = false;
                button.Key.IsEnabled = button.Value.IsEnabled;
            }

            // don't continue if none 
            if (!_navButtons.Any(x => x.Value.Equals(selected)))
                return false;

            // setup new value
            var navButton = _navButtons.First(x => x.Value.Equals(selected));
            navButton.Key.IsChecked = true;
            navButton.Key.IsEnabled = false;

            return true;

        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(NavigationButtonInfo),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    (d as HamburgerMenu).Selected = (NavigationButtonInfo)e.NewValue;
                }));

        public bool IsOpen
        {
            get
            {
                var open = ShellSplitView.IsPaneOpen;
                if (open != (bool)GetValue(IsOpenProperty))
                    SetValue(IsOpenProperty, open);
                return open;
            }
            set
            {
                var open = ShellSplitView.IsPaneOpen;
                if (open == value)
                    return;
                if (value)
                {
                    ShellSplitView.IsPaneOpen = true;
                }
                else
                {
                    // collapse the window
                    if (ShellSplitView.DisplayMode == SplitViewDisplayMode.Overlay && ShellSplitView.IsPaneOpen)
                        ShellSplitView.IsPaneOpen = false;
                    else if (ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactOverlay && ShellSplitView.IsPaneOpen)
                        ShellSplitView.IsPaneOpen = false;
                }
                SetValue(IsOpenProperty, value);
            }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool),
                typeof(HamburgerMenu), new PropertyMetadata(false,
                    (d, e) => { (d as HamburgerMenu).IsOpen = (bool)e.NewValue; }));

        public NavigationButtonInfoCollection PrimaryButtons
        {
            get
            {
                var PrimaryButtons = (NavigationButtonInfoCollection)base.GetValue(PrimaryButtonsProperty);
                if (PrimaryButtons == null)
                    base.SetValue(PrimaryButtonsProperty, PrimaryButtons = new NavigationButtonInfoCollection(this));
                return PrimaryButtons;
            }
            set
            {
                var oldValue = PrimaryButtons;
                if (oldValue != null)
                {
                    oldValue.Owner = null;
                }
                if (value != null)
                {
                    value.Owner = this;
                }
                SetValue(PrimaryButtonsProperty, value);
            }
        }
        public static readonly DependencyProperty PrimaryButtonsProperty =
            DependencyProperty.Register(nameof(PrimaryButtons), typeof(NavigationButtonInfoCollection),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        private NavigationService _navigationService;
        public NavigationService NavigationService
        {
            get { return _navigationService; }
            set
            {
                _navigationService = value;
                if (NavigationService.Frame.BackStackDepth > 0)
                {
                    // display content inside the splitview
                    ShellSplitView.Content = NavigationService.Frame;
                }
                else
                {
                    // display content without splitview (splash scenario)
                    NavigationService.AfterRestoreSavedNavigation += (s, e) => IsFullScreen = false;
                    NavigationService.FrameFacade.Navigated += (s, e) => IsFullScreen = false;
                    IsFullScreen = true;
                }
                NavigationService.FrameFacade.Navigated += (s, e) => UpdateButtons(e);
                NavigationService.AfterRestoreSavedNavigation += (s, e) => UpdateButtons();
                ShellSplitView.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (s, e) =>
                {
                    // update width
                    PaneWidth = !ShellSplitView.IsPaneOpen ? ShellSplitView.CompactPaneLength : ShellSplitView.OpenPaneLength;
                });
            }
        }

        public bool IsFullScreen
        {
            get { return (bool)GetValue(IsFullScreenProperty); }
            set { SetValue(IsFullScreenProperty, value); }
        }
        public static readonly DependencyProperty IsFullScreenProperty =
            DependencyProperty.Register(nameof(IsFullScreen), typeof(bool),
                typeof(HamburgerMenu), new PropertyMetadata(false, (d, e) =>
                {
                    var menu = d as HamburgerMenu;
                    if ((bool)e.NewValue)
                    {
                        if (menu.RootGrid.Children.Contains(menu.NavigationService.Frame))
                            return;
                        menu.NavigationService.Frame.SetValue(Grid.ColumnProperty, 0);
                        menu.NavigationService.Frame.SetValue(Grid.ColumnSpanProperty, int.MaxValue);
                        menu.NavigationService.Frame.SetValue(Grid.RowProperty, 0);
                        menu.NavigationService.Frame.SetValue(Grid.RowSpanProperty, int.MaxValue);
                        menu.RootGrid.Children.Add(menu.NavigationService.Frame);
                    }
                    else
                    {
                        if (menu.RootGrid.Children.Contains(menu.NavigationService.Frame))
                            menu.RootGrid.Children.Remove(menu.NavigationService.Frame);
                        menu.ShellSplitView.Content = menu.NavigationService.Frame;
                    }
                }));


        public NavigationButtonInfoCollection SecondaryButtons
        {
            get
            {
                var SecondaryButtons = (NavigationButtonInfoCollection)base.GetValue(SecondaryButtonsProperty);
                if (SecondaryButtons == null)
                    base.SetValue(SecondaryButtonsProperty, SecondaryButtons = new NavigationButtonInfoCollection(this));
                return SecondaryButtons;
            }
            set
            {
                var oldValue = SecondaryButtons;
                if (oldValue != null)
                {
                    oldValue.Owner = null;
                }
                if (value != null)
                {
                    value.Owner = this;
                }
                SetValue(SecondaryButtonsProperty, value);
            }
        }
        public static readonly DependencyProperty SecondaryButtonsProperty =
            DependencyProperty.Register(nameof(SecondaryButtons), typeof(NavigationButtonInfoCollection),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public double PaneWidth
        {
            get { return (double)GetValue(PaneWidthProperty); }
            set { SetValue(PaneWidthProperty, value); }
        }
        public static readonly DependencyProperty PaneWidthProperty =
            DependencyProperty.Register("PaneWidth", typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata(220));

        public event PropertyChangedEventHandler PropertyChanged;

        public UIElement HeaderContent
        {
            get { return (UIElement)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register(nameof(HeaderContent), typeof(UIElement),
                typeof(HamburgerMenu), null);

        Dictionary<RadioButton, NavigationButtonInfo> _navButtons = new Dictionary<RadioButton, NavigationButtonInfo>();
        void NavButton_Loaded(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            _navButtons.Add(radio, radio.DataContext as NavigationButtonInfo);
            UpdateButtons();
        }

        private void PaneContent_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            HamburgerCommand.Execute(null);
        }

        private void NavButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }

    public class NavigationButtonInfoCollection : ObservableCollection<NavigationButtonInfo>
    {
        public NavigationButtonInfoCollection()
        {
        }

        internal NavigationButtonInfoCollection(HamburgerMenu owner) : this()
        {
            Owner = owner;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (INotifyPropertyChanged item in e.NewItems)
                    {
                        item.PropertyChanged += itemPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (INotifyPropertyChanged item in e.OldItems)
                    {
                        item.PropertyChanged -= itemPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (INotifyPropertyChanged item in e.OldItems)
                    {
                        item.PropertyChanged -= itemPropertyChanged;
                    }
                    foreach (INotifyPropertyChanged item in e.NewItems)
                    {
                        item.PropertyChanged += itemPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
            base.OnCollectionChanged(e);
        }

        protected override void ClearItems()
        {
            foreach (INotifyPropertyChanged item in this)
            {
                item.PropertyChanged -= itemPropertyChanged;
            }
            base.ClearItems();
        }

        private void itemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Owner.UpdateNavButtons(Owner.Selected);
        }

        internal HamburgerMenu Owner { get; set; }

    }

    [ContentProperty(Name = nameof(Content))]
    public class NavigationButtonInfo : INotifyPropertyChanged
    {
        private Visibility _visibility = Visibility.Visible;
        private bool _isEnabled = true;

        /// <summary>
        /// Sets and gets the PageType property.
        /// </summary>
        public Type PageType { get; set; }
        /// <summary>
        /// Sets and gets the PageParameter property.
        /// </summary>
        public object PageParameter { get; set; }
        /// <summary>
        /// Sets and gets the ClearHistory property.
        /// If true, navigation stack is cleared when navigating to this page
        /// </summary>
        public bool ClearHistory { get; set; } = false;

        /// <summary>
        /// Sets and gets the Visibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }

            set
            {
                if (_visibility == value)
                {
                    return;
                }
                _visibility = value;
                RaisePropertyChanged(nameof(Visibility));
            }
        }

        /// <summary>
        /// Sets and gets the IsEnabled property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled == value)
                {
                    return;
                }
                _isEnabled = value;
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        public UIElement Content { get; set; }

        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return string.Format("{0}({1})", PageType, PageParameter);
        }
    }
}
