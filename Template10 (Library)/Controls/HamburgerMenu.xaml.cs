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
                PrimaryButtons = new ObservableItemCollection<HamburgerButtonInfo>();
                SecondaryButtons = new ObservableItemCollection<HamburgerButtonInfo>();
                new KeyboardService().AfterWindowZGesture = () => { HamburgerCommand.Execute(null); };
                ShellSplitView.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (d, e) =>
                {
                    if (SecondaryButtonOrientation.Equals(Orientation.Horizontal) && ShellSplitView.IsPaneOpen)
                        _SecondaryButtonStackPanel.Orientation = Orientation.Horizontal;
                    else
                        _SecondaryButtonStackPanel.Orientation = Orientation.Vertical;
                });
            }
        }

        public void HighlightCorrectButton()
        {
            var pageType = NavigationService.CurrentPageType;
            var pageParam = NavigationService.CurrentPageParam;
            var values = _navButtons.Select(x => x.Value);
            var button = values.FirstOrDefault(x => x.PageType == pageType && x.PageParameter == pageParam);
            Selected = button;
        }

        #region commands

        Mvvm.DelegateCommand _hamburgerCommand;
        internal Mvvm.DelegateCommand HamburgerCommand { get { return _hamburgerCommand ?? (_hamburgerCommand = new Mvvm.DelegateCommand(ExecuteHamburger)); } }
        void ExecuteHamburger() { IsOpen = !IsOpen; }

        Mvvm.DelegateCommand<HamburgerButtonInfo> _navCommand;
        public Mvvm.DelegateCommand<HamburgerButtonInfo> NavCommand { get { return _navCommand ?? (_navCommand = new Mvvm.DelegateCommand<HamburgerButtonInfo>(ExecuteNav)); } }
        void ExecuteNav(HamburgerButtonInfo commandInfo)
        {
            if (commandInfo == null)
                throw new NullReferenceException("CommandParameter is not set");
            try
            {
                if (commandInfo.PageType != null)
                    Selected = commandInfo;
            }
            finally
            {
                if (commandInfo.ClearHistory)
                    NavigationService.ClearHistory();
            }
        }

        #endregion

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

        public Orientation SecondaryButtonOrientation
        {
            get { return (Orientation)GetValue(SecondaryButtonOrientationProperty); }
            set { SetValue(SecondaryButtonOrientationProperty, value); }
        }
        public static readonly DependencyProperty SecondaryButtonOrientationProperty =
            DependencyProperty.Register(nameof(SecondaryButtonOrientation), typeof(Orientation),
                typeof(HamburgerMenu), new PropertyMetadata(Orientation.Vertical));

        public SolidColorBrush HamburgerBackground
        {
            get { return HamburgerBackgroundBrush; }
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

        public HamburgerButtonInfo Selected
        {
            get { return GetValue(SelectedProperty) as HamburgerButtonInfo; }
            set
            {
                IsOpen = false;

                // ensure dp is correct (if diff)
                var previous = Selected;
                if (previous != value)
                {
                    SetValue(SelectedProperty, value);
                    // undo previous
                    if (previous != null)
                    {
                        previous.RaiseUnselected();
                    }
                }

                // reset all
                var values = _navButtons.Select(x => x.Value);
                foreach (var item in values)
                {
                    item.IsEnabled = true;
                    item.IsChecked = false;
                }

                // that's it if null
                if (value == null)
                    return;

                // setup new value
                value.IsChecked = true;
                if (previous != value)
                    value.RaiseSelected();

                // navigate only to new pages
                if (value.PageType != null && (NavigationService.CurrentPageType != value.PageType || NavigationService.CurrentPageParam != value.PageParameter))
                {
                    NavigationService.Navigate(value.PageType, value.PageParameter);
                    value.IsEnabled = false;
                    HighlightCorrectButton();
                }
            }
        }
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(HamburgerButtonInfo),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    if ((d as HamburgerMenu).Selected != (HamburgerButtonInfo)e.NewValue)
                        (d as HamburgerMenu).Selected = (HamburgerButtonInfo)e.NewValue;
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

        public ObservableItemCollection<HamburgerButtonInfo> PrimaryButtons
        {
            get
            {
                var PrimaryButtons = (ObservableItemCollection<HamburgerButtonInfo>)base.GetValue(PrimaryButtonsProperty);
                if (PrimaryButtons == null)
                    base.SetValue(PrimaryButtonsProperty, PrimaryButtons = new ObservableItemCollection<HamburgerButtonInfo>());
                return PrimaryButtons;
            }
            set { SetValue(PrimaryButtonsProperty, value); }
        }
        public static readonly DependencyProperty PrimaryButtonsProperty =
            DependencyProperty.Register(nameof(PrimaryButtons), typeof(ObservableItemCollection<HamburgerButtonInfo>),
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
                NavigationService.FrameFacade.Navigated += (s, e) => HighlightCorrectButton();
                NavigationService.AfterRestoreSavedNavigation += (s, e) => HighlightCorrectButton();
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


        public ObservableItemCollection<HamburgerButtonInfo> SecondaryButtons
        {
            get
            {
                var SecondaryButtons = (ObservableItemCollection<HamburgerButtonInfo>)base.GetValue(SecondaryButtonsProperty);
                if (SecondaryButtons == null)
                    base.SetValue(SecondaryButtonsProperty, SecondaryButtons = new ObservableItemCollection<HamburgerButtonInfo>());
                return SecondaryButtons;
            }
            set { SetValue(SecondaryButtonsProperty, value); }
        }
        public static readonly DependencyProperty SecondaryButtonsProperty =
            DependencyProperty.Register(nameof(SecondaryButtons), typeof(ObservableItemCollection<HamburgerButtonInfo>),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public double PaneWidth
        {
            get { return (double)GetValue(PaneWidthProperty); }
            set { SetValue(PaneWidthProperty, value); }
        }
        public static readonly DependencyProperty PaneWidthProperty =
            DependencyProperty.Register("PaneWidth", typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata(220d));

        public UIElement HeaderContent
        {
            get { return (UIElement)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register(nameof(HeaderContent), typeof(UIElement),
                typeof(HamburgerMenu), null);

        Dictionary<RadioButton, HamburgerButtonInfo> _navButtons = new Dictionary<RadioButton, HamburgerButtonInfo>();
        void NavButton_Loaded(object sender, RoutedEventArgs e)
        {
            // add this radio to the list
            var radio = sender as RadioButton;
            var info = radio.DataContext as HamburgerButtonInfo;
            _navButtons.Add(radio, info);

            // map clicked
            radio.Checked += (s, args) => info.RaiseChecked(args);
            radio.Unchecked += (s, args) => info.RaiseUnchecked(args);

            // udpate UI
            HighlightCorrectButton();
        }

        private void PaneContent_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            HamburgerCommand.Execute(null);
        }

        private void NavButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            var info = radio.DataContext as HamburgerButtonInfo;
            info.RaiseTapped(e);

            // why is it handled?
            // so we don't re-select
            e.Handled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        StackPanel _SecondaryButtonStackPanel;
        private void SecondaryButtonStackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            _SecondaryButtonStackPanel = sender as StackPanel;
        }
    }
}
