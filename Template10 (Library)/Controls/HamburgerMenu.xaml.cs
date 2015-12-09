using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Template10.Common;
using Template10.Services.KeyboardService;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-HamburgerMenu
    [ContentProperty(Name = nameof(PrimaryButtons))]
    public sealed partial class HamburgerMenu : UserControl, INotifyPropertyChanged
    {
        public event EventHandler PaneOpen;
        public event EventHandler PaneClosed;
        public event EventHandler<ChangedEventArgs<HamburgerButtonInfo>> SelectedChanged;

        public HamburgerMenu()
        {
            InitializeComponent();
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
                    // secondary layout
                    if (SecondaryButtonOrientation.Equals(Orientation.Horizontal) && ShellSplitView.IsPaneOpen)
                        _SecondaryButtonStackPanel.Orientation = Orientation.Horizontal;
                    else
                        _SecondaryButtonStackPanel.Orientation = Orientation.Vertical;

                    // overall events
                    if (ShellSplitView.IsPaneOpen)
                        PaneOpen?.Invoke(ShellSplitView, EventArgs.Empty);
                    else
                        PaneClosed?.Invoke(ShellSplitView, EventArgs.Empty);
                });
                ShellSplitView.RegisterPropertyChangedCallback(SplitView.DisplayModeProperty, (d, e) =>
                {
                    DisplayMode = ShellSplitView.DisplayMode;
                });
                Loaded += (s, e) =>
                {
                    var any = GetType().GetRuntimeProperties()
                        .Where(x => x.PropertyType == typeof(SolidColorBrush))
                        .Any(x => x.GetValue(this) != null);
                    if (!any)
                        // this is the default color if the user supplies none
                        AccentColor = Colors.DarkOrange;
                };
            }
        }

        public SplitViewDisplayMode DisplayMode
        {
            get { return (SplitViewDisplayMode)GetValue(DisplayModeProperty); }
            private set { SetValue(DisplayModeProperty, value); }
        }
        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register(nameof(DisplayMode), typeof(SplitViewDisplayMode),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public void HighlightCorrectButton(Type pageType = null, object pageParam = null)
        {
            pageType = pageType ?? NavigationService.CurrentPageType;
            pageParam = pageParam ?? NavigationService.CurrentPageParam;
            var values = _navButtons.Select(x => x.Value);
            var button = values.FirstOrDefault(x => x.PageType == pageType && 
                                                (x.PageParameter == null || x.PageParameter.Equals(pageParam)));
            Selected = button;
        }

        #region commands

        Mvvm.DelegateCommand _hamburgerCommand;
        internal Mvvm.DelegateCommand HamburgerCommand => _hamburgerCommand ?? (_hamburgerCommand = new Mvvm.DelegateCommand(ExecuteHamburger));
        void ExecuteHamburger() { IsOpen = !IsOpen; }

        Mvvm.DelegateCommand<HamburgerButtonInfo> _navCommand;
        public Mvvm.DelegateCommand<HamburgerButtonInfo> NavCommand => _navCommand ?? (_navCommand = new Mvvm.DelegateCommand<HamburgerButtonInfo>(ExecuteNav));
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

        public enum VisualStateSettings { Narrow, Normal, Wide, Auto }
        public VisualStateSettings VisualStateSetting
        {
            get { return (VisualStateSettings)GetValue(VisualStateSettingProperty); }
            set { SetValue(VisualStateSettingProperty, value); }
        }
        public static readonly DependencyProperty VisualStateSettingProperty =
            DependencyProperty.Register("VisualStateSetting", typeof(VisualStateSettings),
                typeof(HamburgerMenu), new PropertyMetadata(VisualStateSettings.Auto, VisualStateSettingChanged));
        private static void VisualStateSettingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            switch ((VisualStateSettings)e.NewValue)
            {
                case VisualStateSettings.Narrow:
                case VisualStateSettings.Normal:
                case VisualStateSettings.Wide:
                    throw new NotImplementedException();
                case VisualStateSettings.Auto:
                    break;
            }
        }

        public double VisualStateNarrowMinWidth
        {
            get { return VisualStateNarrowTrigger.MinWindowWidth; }
            set { SetValue(VisualStateNarrowMinWidthProperty, VisualStateNarrowTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateNarrowMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNarrowMinWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata((double)-1, (d, e) => { Changed(nameof(VisualStateNarrowMinWidth), e); }));

        public double VisualStateNormalMinWidth
        {
            get { return VisualStateNormalTrigger.MinWindowWidth; }
            set { SetValue(VisualStateNormalMinWidthProperty, VisualStateNormalTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateNormalMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNormalMinWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata((double)0, (d, e) => { Changed(nameof(VisualStateNormalMinWidth), e); }));

        public double VisualStateWideMinWidth
        {
            get { return VisualStateWideTrigger.MinWindowWidth; }
            set { SetValue(VisualStateWideMinWidthProperty, VisualStateWideTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateWideMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateWideMinWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata((double)-1, (d, e) => { Changed(nameof(VisualStateWideMinWidth), e); }));

        private static void Changed(string v, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region Style Properties

        public Orientation SecondaryButtonOrientation
        {
            get { return (Orientation)GetValue(SecondaryButtonOrientationProperty); }
            set { SetValue(SecondaryButtonOrientationProperty, value); }
        }
        public static readonly DependencyProperty SecondaryButtonOrientationProperty =
            DependencyProperty.Register(nameof(SecondaryButtonOrientation), typeof(Orientation),
                typeof(HamburgerMenu), new PropertyMetadata(Orientation.Vertical));

        public Color AccentColor
        {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }
        public static readonly DependencyProperty AccentColorProperty =
            DependencyProperty.Register(nameof(AccentColor), typeof(Color),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => (d as HamburgerMenu).RefreshStyles((Color)e.NewValue)));

        public void RefreshStyles(ApplicationTheme? theme = null)
        {
            RequestedTheme = theme?.ToElementTheme() ?? RequestedTheme;
            RefreshStyles(AccentColor);
        }

        public void RefreshStyles(Color? color = null)
        {
            if (color == null)
            {
                // manually setting the brushes is a way of ignoring the themes
                // in this block we will unset, then re-set the values

                var hamburgerBackground = HamburgerBackground;
                var hamburgerForeground = HamburgerForeground;
                var navAreaBackground = NavAreaBackground;
                var navButtonBackground = NavButtonBackground;
                var navButtonForeground = NavButtonForeground;
                var navButtonCheckedBackground = NavButtonCheckedBackground;
                var navButtonPressedBackground = NavButtonPressedBackground;
                var navButtonHoverBackground = NavButtonHoverBackground;
                var navButtonCheckedForeground = NavButtonCheckedForeground;
                var secondarySeparator = SecondarySeparator;

                HamburgerBackground = null;
                HamburgerForeground = null;
                NavAreaBackground = null;
                NavButtonBackground = null;
                NavButtonForeground = null;
                NavButtonCheckedBackground = null;
                NavButtonPressedBackground = null;
                NavButtonHoverBackground = null;
                NavButtonCheckedForeground = null;
                SecondarySeparator = null;

                HamburgerBackground = hamburgerBackground;
                HamburgerForeground = hamburgerForeground;
                NavAreaBackground = navAreaBackground;
                NavButtonBackground = navButtonBackground;
                NavButtonForeground = navButtonForeground;
                NavButtonCheckedBackground = navButtonCheckedBackground;
                NavButtonPressedBackground = navButtonPressedBackground;
                NavButtonHoverBackground = navButtonHoverBackground;
                NavButtonCheckedForeground = navButtonCheckedForeground;
                SecondarySeparator = secondarySeparator;
            }
            else
            {
                // since every brush will be based on one color,
                // we will do so with theme in mind.

                switch (RequestedTheme)
                {
                    case ElementTheme.Light:
                        HamburgerBackground = color?.ToSolidColorBrush();
                        HamburgerForeground = Colors.White.ToSolidColorBrush();
                        NavAreaBackground = Colors.DimGray.ToSolidColorBrush();
                        NavButtonBackground = Colors.Transparent.ToSolidColorBrush();
                        NavButtonForeground = Colors.White.ToSolidColorBrush();
                        NavButtonCheckedForeground = Colors.White.ToSolidColorBrush();
                        NavButtonCheckedBackground = color?.Lighten(ColorUtils.Accents.Plus20).ToSolidColorBrush();
                        NavButtonPressedBackground = Colors.Gainsboro.Darken(ColorUtils.Accents.Plus40).ToSolidColorBrush();
                        NavButtonHoverBackground = Colors.Gainsboro.Darken(ColorUtils.Accents.Plus60).ToSolidColorBrush();
                        NavButtonCheckedForeground = Colors.White.ToSolidColorBrush();
                        SecondarySeparator = Colors.Gainsboro.Darken(ColorUtils.Accents.Plus40).ToSolidColorBrush();
                        break;
                    case ElementTheme.Default:
                    case ElementTheme.Dark:
                        HamburgerBackground = color?.ToSolidColorBrush();
                        HamburgerForeground = Colors.White.ToSolidColorBrush();
                        NavAreaBackground = Colors.Gainsboro.Darken(ColorUtils.Accents.Plus80).ToSolidColorBrush();
                        NavButtonBackground = Colors.Transparent.ToSolidColorBrush();
                        NavButtonForeground = Colors.White.ToSolidColorBrush();
                        NavButtonCheckedForeground = Colors.White.ToSolidColorBrush();
                        NavButtonCheckedBackground = color?.Darken(ColorUtils.Accents.Plus40).ToSolidColorBrush();
                        NavButtonPressedBackground = Colors.Gainsboro.Lighten(ColorUtils.Accents.Plus40).ToSolidColorBrush();
                        NavButtonHoverBackground = Colors.Gainsboro.Lighten(ColorUtils.Accents.Plus60).ToSolidColorBrush();
                        NavButtonCheckedForeground = Colors.White.ToSolidColorBrush();
                        SecondarySeparator = Colors.Gainsboro.ToSolidColorBrush();
                        break;
                }
            }
        }

        public SolidColorBrush HamburgerBackground
        {
            get { return GetValue(HamburgerBackgroundProperty) as SolidColorBrush; }
            set { SetValue(HamburgerBackgroundProperty, value); }
        }
        public static readonly DependencyProperty HamburgerBackgroundProperty =
            DependencyProperty.Register(nameof(HamburgerBackground), typeof(SolidColorBrush),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public SolidColorBrush HamburgerForeground
        {
            get { return GetValue(HamburgerForegroundProperty) as SolidColorBrush; }
            set { SetValue(HamburgerForegroundProperty, value); }
        }
        public static readonly DependencyProperty HamburgerForegroundProperty =
              DependencyProperty.Register(nameof(HamburgerForeground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null));

        public SolidColorBrush NavAreaBackground
        {
            get { return GetValue(NavAreaBackgroundProperty) as SolidColorBrush; }
            set { SetValue(NavAreaBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavAreaBackgroundProperty =
              DependencyProperty.Register(nameof(NavAreaBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null));

        public SolidColorBrush NavButtonBackground
        {
            get { return GetValue(NavButtonBackgroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonBackgroundProperty =
            DependencyProperty.Register(nameof(NavButtonBackground), typeof(SolidColorBrush),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public SolidColorBrush NavButtonForeground
        {
            get { return GetValue(NavButtonForegroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonForegroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonForegroundProperty =
            DependencyProperty.Register(nameof(NavButtonForeground), typeof(SolidColorBrush),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public SolidColorBrush SecondarySeparator
        {
            get { return GetValue(SecondarySeparatorProperty) as SolidColorBrush; }
            set { SetValue(SecondarySeparatorProperty, value); }
        }
        public static readonly DependencyProperty SecondarySeparatorProperty =
              DependencyProperty.Register(nameof(SecondarySeparator), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null));

        public SolidColorBrush NavButtonCheckedBackground
        {
            get { return GetValue(NavButtonCheckedBackgroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonCheckedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonCheckedBackgroundProperty =
              DependencyProperty.Register(nameof(NavButtonCheckedBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null));

        public SolidColorBrush NavButtonCheckedForeground
        {
            get { return GetValue(NavButtonCheckedForegroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonCheckedForegroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonCheckedForegroundProperty =
              DependencyProperty.Register(nameof(NavButtonCheckedForeground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null));

        public SolidColorBrush NavButtonPressedBackground
        {
            get { return GetValue(NavButtonPressedBackgroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonPressedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonPressedBackgroundProperty =
              DependencyProperty.Register(nameof(NavButtonPressedBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null));

        public SolidColorBrush NavButtonHoverBackground
        {
            get { return GetValue(NavButtonHoverBackgroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonHoverBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonHoverBackgroundProperty =
              DependencyProperty.Register(nameof(NavButtonHoverBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null));

        #endregion

        #region Properties

        public HamburgerButtonInfo Selected
        {
            get { return GetValue(SelectedProperty) as HamburgerButtonInfo; }
            set
            {
                HamburgerButtonInfo oldValue = Selected;
                if (value?.Equals(oldValue) ?? false)
                    value.IsChecked = (value.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);
                SetValue(SelectedProperty, value);
                this.SelectedChanged?.Invoke(this, new ChangedEventArgs<HamburgerButtonInfo>(oldValue, value));
            }
        }
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register(nameof(Selected), typeof(HamburgerButtonInfo),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                { (d as HamburgerMenu).SetSelected((HamburgerButtonInfo)e.OldValue, (HamburgerButtonInfo)e.NewValue); }));
        private void SetSelected(HamburgerButtonInfo previous, HamburgerButtonInfo value)
        {
            IsOpen = false;

            // undo previous
            if (previous != null && previous != value)
            {
                previous.RaiseUnselected();
            }

            // reset all
            var values = _navButtons.Select(x => x.Value);
            foreach (var item in values.Where(x => x != value))
            {
                item.IsChecked = false;
            }

            // that's it if null
            if (value == null)
            {
                return;
            }
            else
            {
                value.IsChecked = (value.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);
                if (previous != value)
                {
                    value.RaiseSelected();
                }
            }

            // navigate only to new pages
            if (value.PageType == null) return;
            if (value.PageType.Equals(NavigationService.CurrentPageType) && (value.PageParameter?.Equals(NavigationService.CurrentPageParam) ?? false)) return;
            NavigationService.Navigate(value.PageType, value.PageParameter);
        }

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

        public ObservableCollection<HamburgerButtonInfo> PrimaryButtons
        {
            get
            {
                var PrimaryButtons = (ObservableCollection<HamburgerButtonInfo>)base.GetValue(PrimaryButtonsProperty);
                if (PrimaryButtons == null)
                    base.SetValue(PrimaryButtonsProperty, PrimaryButtons = new ObservableCollection<HamburgerButtonInfo>());
                return PrimaryButtons;
            }
            set { SetValue(PrimaryButtonsProperty, value); }
        }
        public static readonly DependencyProperty PrimaryButtonsProperty =
            DependencyProperty.Register(nameof(PrimaryButtons), typeof(ObservableCollection<HamburgerButtonInfo>),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        private INavigationService _navigationService;
        public INavigationService NavigationService
        {
            get { return _navigationService; }
            set
            {
                _navigationService = value;

                #region BottomAppBar

                var bottomAppBarDelay = 1000;
                NavigationService.FrameFacade.Navigated += (s, e) =>
                {
                    StartAppBar(e.Page);
                    WindowWrapper.Current().Dispatcher.Dispatch(() => { StartAppBar(e.Page); }, bottomAppBarDelay);
                };
                NavigationService.FrameFacade.Navigating += (s, e) => StopAppBar(e.Page);
                NavigationService.AfterRestoreSavedNavigation += (s, e) =>
                {
                    StartAppBar(NavigationService.FrameFacade.Content as Page);
                    WindowWrapper.Current().Dispatcher.Dispatch(() => { StartAppBar(NavigationService.FrameFacade.Content as Page); }, bottomAppBarDelay);
                };
                StartAppBar(NavigationService.FrameFacade.Content as Page);
                WindowWrapper.Current().Dispatcher.Dispatch(() => { StartAppBar(NavigationService.FrameFacade.Content as Page); }, bottomAppBarDelay);

                #endregion

                if (NavigationService.Frame.BackStackDepth > 0)
                {
                    // display content inside the splitview
                    ShellSplitView.Content = NavigationService.Frame;
                }
                else
                {
                    // display content without splitview (splash scenario)
                    NavigationService.AfterRestoreSavedNavigation += (s, e) => UpdateFullScreen(IsFullScreen);
                    NavigationService.FrameFacade.Navigated += (s, e) => UpdateFullScreen(IsFullScreen);
                    UpdateFullScreen(true);
                }
                NavigationService.AfterRestoreSavedNavigation += (s, e) => HighlightCorrectButton();
                NavigationService.FrameFacade.Navigated += (s, e) => HighlightCorrectButton(e.PageType, e.Parameter);
                UpdateFullScreen();
            }
        }

        public bool IsFullScreen
        {
            get { return (bool)GetValue(IsFullScreenProperty); }
            set { SetValue(IsFullScreenProperty, value); }
        }
        public static readonly DependencyProperty IsFullScreenProperty =
            DependencyProperty.Register(nameof(IsFullScreen), typeof(bool),
                typeof(HamburgerMenu), new PropertyMetadata(false, (d, e) => (d as HamburgerMenu).UpdateFullScreen()));
        private void UpdateFullScreen(bool? manual = null)
        {
            if (manual ?? IsFullScreen)
            {
                if (NavigationService == null || RootGrid.Children.Contains(NavigationService.Frame))
                    return;
                NavigationService.Frame.SetValue(Grid.ColumnProperty, 0);
                ShellSplitView.Content = null;
                ShellSplitView.Visibility = Visibility.Collapsed;
                HamburgerButton.Width = 0;
                NavigationService.Frame.SetValue(Grid.ColumnSpanProperty, int.MaxValue);
                NavigationService.Frame.SetValue(Grid.RowProperty, 0);
                NavigationService.Frame.SetValue(Grid.RowSpanProperty, int.MaxValue);
                RootGrid.Children.Add(NavigationService.Frame);
            }
            else
            {
                if (RootGrid.Children.Contains(NavigationService.Frame))
                    RootGrid.Children.Remove(NavigationService.Frame);
                HamburgerButton.Width = 48;
                ShellSplitView.Visibility = Visibility.Visible;
                ShellSplitView.Content = NavigationService.Frame;
            }
        }

        public ObservableCollection<HamburgerButtonInfo> SecondaryButtons
        {
            get
            {
                var SecondaryButtons = (ObservableCollection<HamburgerButtonInfo>)base.GetValue(SecondaryButtonsProperty);
                if (SecondaryButtons == null)
                    base.SetValue(SecondaryButtonsProperty, SecondaryButtons = new ObservableCollection<HamburgerButtonInfo>());
                return SecondaryButtons;
            }
            set { SetValue(SecondaryButtonsProperty, value); }
        }
        public static readonly DependencyProperty SecondaryButtonsProperty =
            DependencyProperty.Register(nameof(SecondaryButtons), typeof(ObservableCollection<HamburgerButtonInfo>),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public double PaneWidth
        {
            get { return (double)GetValue(PaneWidthProperty); }
            set { SetValue(PaneWidthProperty, value); }
        }
        public static readonly DependencyProperty PaneWidthProperty =
            DependencyProperty.Register(nameof(PaneWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata(220d));

        public UIElement HeaderContent
        {
            get { return (UIElement)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register(nameof(HeaderContent), typeof(UIElement),
                typeof(HamburgerMenu), null);

        #endregion

        #region BottomAppBar

        private void StartAppBar(Page page)
        {
            if (page?.BottomAppBar != null)
            {
                page.BottomAppBar.Opened -= BottomAppBar_Handler;
                page.BottomAppBar.Opened += BottomAppBar_Handler;
                page.BottomAppBar.Closing -= BottomAppBar_Handler;
                page.BottomAppBar.Closing += BottomAppBar_Handler;
                UpdateAppBar(page.BottomAppBar);
            }
        }

        private void StopAppBar(Page page)
        {
            if (page?.BottomAppBar != null)
            {
                page.BottomAppBar.Opened -= BottomAppBar_Handler;
                page.BottomAppBar.Closing -= BottomAppBar_Handler;
                UpdateAppBar(null);
            }
        }

        private void BottomAppBar_Handler(object sender, object e)
        {
            UpdateAppBar(sender as AppBar);
        }

        private void UpdateAppBar(AppBar appbar)
        {
            var height = 0d;
            if (appbar == null)
                height = 0d;
            else if (appbar.Visibility == Visibility.Collapsed)
                height = 0d;
            else if (appbar.IsOpen)
                height = ((appbar.Content as FrameworkElement)?.ActualHeight ?? appbar.ActualHeight);
            else
                height = appbar.ActualHeight;
            PaneContent.Margin = new Thickness(PaneContent.Margin.Left, PaneContent.Margin.Top, PaneContent.Margin.Right, height);
            PaneContent.InvalidateMeasure();
        }

        #endregion

        Dictionary<RadioButton, HamburgerButtonInfo> _navButtons = new Dictionary<RadioButton, HamburgerButtonInfo>();
        void NavButton_Loaded(object sender, RoutedEventArgs e)
        {
            // add this radio to the list
            var r = sender as RadioButton;
            var i = r.DataContext as HamburgerButtonInfo;
            _navButtons.Add(r, i);
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

        private void NavButton_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            var info = radio.DataContext as HamburgerButtonInfo;
            info.RaiseRightTapped(e);

            e.Handled = true;
        }

        private void NavButton_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            var info = radio.DataContext as HamburgerButtonInfo;
            info.RaiseHolding(e);

            e.Handled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        StackPanel _SecondaryButtonStackPanel;
        private void SecondaryButtonStackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            _SecondaryButtonStackPanel = sender as StackPanel;
        }

        private void NavButtonChecked(object sender, RoutedEventArgs e)
        {
            var t = sender as ToggleButton;
            var i = t.DataContext as HamburgerButtonInfo;
            t.IsChecked = (i.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);
            i.RaiseChecked(e);
            HighlightCorrectButton();
        }

        private void NavButtonUnchecked(object sender, RoutedEventArgs e)
        {
            var t = sender as ToggleButton;
            var i = t.DataContext as HamburgerButtonInfo;
            i.RaiseUnchecked(e);
            HighlightCorrectButton();
        }
    }
}
