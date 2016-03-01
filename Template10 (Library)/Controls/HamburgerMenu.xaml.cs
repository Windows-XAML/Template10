using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Template10.Common;
using Template10.Services.KeyboardService;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.Foundation;
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
    public sealed partial class HamburgerMenu : UserControl
    {
        [Obsolete("Fixing naming inconsistency; use HamburgerMenu.PaneOpened", true)]
        public event EventHandler PaneOpen;
        public event EventHandler PaneOpened;
        public event EventHandler PaneClosed;
        public event EventHandler<ChangedEventArgs<HamburgerButtonInfo>> SelectedChanged;

        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"HamburgerMenu.{caller}");

        private static void Changed(string v, DependencyPropertyChangedEventArgs e)
        {
            DebugWrite($"OldValue: {e.OldValue} NewValue: {e.NewValue}", caller: v);
        }

        #endregion

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
                KeyboardService.Instance.AfterWindowZGesture = () => { HamburgerCommand.Execute(null); };
                ShellSplitView.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (d, e) =>
                {
                    DebugWrite($"Current:{(d as SplitView).IsPaneOpen}");

                    // this can occur if the user resizes before it loads
                    if (_SecondaryButtonStackPanel == null)
                        return;

                    // secondary layout
                    if (SecondaryButtonOrientation.Equals(Orientation.Horizontal)
                        && ShellSplitView.IsPaneOpen)
                        _SecondaryButtonStackPanel.Orientation = Orientation.Horizontal;
                    else
                        _SecondaryButtonStackPanel.Orientation = Orientation.Vertical;

                    // overall events
                    if ((d as SplitView).IsPaneOpen)
                    {
                        PaneOpened?.Invoke(ShellSplitView, EventArgs.Empty);
                        PaneOpen?.Invoke(ShellSplitView, EventArgs.Empty);
                    }
                    else
                        PaneClosed?.Invoke(ShellSplitView, EventArgs.Empty);

                    // this will keep the two properties in sync
                    if (!d.GetValue(e).Equals(IsOpen))
                        IsOpen = !IsOpen;
                });
                ShellSplitView.RegisterPropertyChangedCallback(SplitView.DisplayModeProperty, (d, e) =>
                {
                    DebugWrite($"Current:{ShellSplitView.DisplayMode}");

                    // this will keep the two properties in sync
                    DisplayMode = ShellSplitView.DisplayMode;
                });
                Loaded += (s, e) =>
                {
                    // look to see if any brush property has been set
                    var any = GetType().GetRuntimeProperties()
                        .Where(x => x.PropertyType == typeof(SolidColorBrush))
                        .Any(x => x.GetValue(this) != null);

                    // this is the default color if the user supplies none
                    if (!any)
                        AccentColor = (Color)Resources["SystemAccentColor"];

                    // in case the developer has defined zero buttons
                    if (NavButtonCount == 0)
                        _areNavButtonsLoaded = true;
                };
            }
        }

        public SplitViewDisplayMode DisplayMode
        {
            get { return (SplitViewDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }
        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register(nameof(DisplayMode), typeof(SplitViewDisplayMode),
                typeof(HamburgerMenu), new PropertyMetadata(null, DisplayModeChanged));
        private static void DisplayModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugWrite($"Old:{e.OldValue} New:{e.NewValue}");

            var h = d as HamburgerMenu;
            var m = (SplitViewDisplayMode)e.NewValue;
            if (h.ShellSplitView.DisplayMode != m)
                h.ShellSplitView.DisplayMode = m;
        }

        internal void HighlightCorrectButton(Type pageType = null, object pageParam = null)
        {
            DebugWrite($"PageType: {pageType} PageParam: {pageParam}");

            pageType = pageType ?? NavigationService.CurrentPageType;
            var type_match_buttons = _navButtons
                .Where(x => Equals(x.Value.PageType, pageType));

            if (pageParam == null)
                pageParam = NavigationService.CurrentPageParam;
            else
                try
                {
                    pageParam = NavigationService.FrameFacade.SerializationService.Deserialize(pageParam.ToString());
                }
                catch { }

            var param_match_buttons = type_match_buttons
                .Where(x => Equals(x.Value.PageParameter, null) || Equals(x.Value.PageParameter, pageParam));

            var button = param_match_buttons.Select(x => x.Value).FirstOrDefault();

            if (button == null)
                button = type_match_buttons.Select(x => x.Value).FirstOrDefault();

            Selected = button;
        }

        #region commands

        Mvvm.DelegateCommand _hamburgerCommand;
        internal Mvvm.DelegateCommand HamburgerCommand => _hamburgerCommand ?? (_hamburgerCommand = new Mvvm.DelegateCommand(ExecuteHamburger));
        void ExecuteHamburger()
        {
            DebugWrite();

            IsOpen = !IsOpen;
        }

        Mvvm.DelegateCommand<HamburgerButtonInfo> _navCommand;
        public Mvvm.DelegateCommand<HamburgerButtonInfo> NavCommand => _navCommand ?? (_navCommand = new Mvvm.DelegateCommand<HamburgerButtonInfo>(ExecuteNav));
        void ExecuteNav(HamburgerButtonInfo commandInfo)
        {
            DebugWrite($"HamburgerButtonInfo: {commandInfo}");

            if (commandInfo == null)
                throw new NullReferenceException("CommandParameter is not set");

            if (commandInfo.PageType != null)
                Selected = commandInfo;
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

        #endregion

        #region Style Properties

        public Orientation SecondaryButtonOrientation
        {
            get { return (Orientation)GetValue(SecondaryButtonOrientationProperty); }
            set { SetValue(SecondaryButtonOrientationProperty, value); }
        }
        public static readonly DependencyProperty SecondaryButtonOrientationProperty =
            DependencyProperty.Register(nameof(SecondaryButtonOrientation), typeof(Orientation),
                typeof(HamburgerMenu), new PropertyMetadata(Orientation.Vertical, (d, e) => { Changed(nameof(SecondaryButtonOrientation), e); }));

        public Color AccentColor
        {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }
        public static readonly DependencyProperty AccentColorProperty =
            DependencyProperty.Register(nameof(AccentColor), typeof(Color),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    Changed(nameof(AccentColor), e);
                    (d as HamburgerMenu).RefreshStyles((Color)e.NewValue);
                }));

        public void RefreshStyles(ApplicationTheme? theme = null)
        {
            DebugWrite($"Theme: {theme}");

            RequestedTheme = theme?.ToElementTheme() ?? RequestedTheme;
            RefreshStyles(AccentColor);
        }

        public void RefreshStyles(Color? color = null)
        {
            DebugWrite($"Color: {color}");

            if (color != null)
            {
                // since every brush will be based on one color,
                // we will do so with theme in mind.

                switch (RequestedTheme)
                {
                    case ElementTheme.Default:
                    case ElementTheme.Light:
                        {
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
                            SecondarySeparator = PaneBorderBrush = Colors.Gainsboro.Darken(ColorUtils.Accents.Plus40).ToSolidColorBrush();
                        }
                        break;
                    case ElementTheme.Dark:
                        {
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
                            SecondarySeparator = PaneBorderBrush = Colors.Gainsboro.ToSolidColorBrush();
                        }
                        break;
                }
            }
        }

        public Visibility HamburgerButtonVisibility
        {
            get { return (Visibility)GetValue(HamburgerButtonVisibilityProperty); }
            set { SetValue(HamburgerButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty HamburgerButtonVisibilityProperty =
            DependencyProperty.Register(nameof(HamburgerButtonVisibility), typeof(Visibility),
                typeof(HamburgerMenu), new PropertyMetadata(Visibility.Visible, HamburgerButtonVisibilityChanged));
        private static void HamburgerButtonVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Changed(nameof(HamburgerButtonVisibility), e);
            (d as HamburgerMenu).HamburgerButton.Visibility = (Visibility)e.NewValue;
        }

        public SolidColorBrush HamburgerBackground
        {
            get { return GetValue(HamburgerBackgroundProperty) as SolidColorBrush; }
            set { SetValue(HamburgerBackgroundProperty, value); }
        }
        public static readonly DependencyProperty HamburgerBackgroundProperty =
            DependencyProperty.Register(nameof(HamburgerBackground), typeof(SolidColorBrush),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(HamburgerBackground), e)));

        public SolidColorBrush HamburgerForeground
        {
            get { return GetValue(HamburgerForegroundProperty) as SolidColorBrush; }
            set { SetValue(HamburgerForegroundProperty, value); }
        }
        public static readonly DependencyProperty HamburgerForegroundProperty =
              DependencyProperty.Register(nameof(HamburgerForeground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(HamburgerForeground), e)));

        public SolidColorBrush NavAreaBackground
        {
            get { return GetValue(NavAreaBackgroundProperty) as SolidColorBrush; }
            set { SetValue(NavAreaBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavAreaBackgroundProperty =
              DependencyProperty.Register(nameof(NavAreaBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(NavAreaBackground), e)));

        public SolidColorBrush NavButtonBackground
        {
            get { return GetValue(NavButtonBackgroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonBackgroundProperty =
            DependencyProperty.Register(nameof(NavButtonBackground), typeof(SolidColorBrush),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(NavButtonBackground), e)));

        public SolidColorBrush NavButtonForeground
        {
            get { return GetValue(NavButtonForegroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonForegroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonForegroundProperty =
            DependencyProperty.Register(nameof(NavButtonForeground), typeof(SolidColorBrush),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(NavButtonForeground), e)));

        public SolidColorBrush SecondarySeparator
        {
            get { return GetValue(SecondarySeparatorProperty) as SolidColorBrush; }
            set { SetValue(SecondarySeparatorProperty, value); }
        }
        public static readonly DependencyProperty SecondarySeparatorProperty =
              DependencyProperty.Register(nameof(SecondarySeparator), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(SecondarySeparator), e)));

        public SolidColorBrush PaneBorderBrush
        {
            get { return GetValue(PaneBorderBrushProperty) as SolidColorBrush; }
            set { SetValue(PaneBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty PaneBorderBrushProperty =
              DependencyProperty.Register(nameof(PaneBorderBrush), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(PaneBorderBrush), e)));

        public SolidColorBrush NavButtonCheckedBackground
        {
            get { return GetValue(NavButtonCheckedBackgroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonCheckedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonCheckedBackgroundProperty =
              DependencyProperty.Register(nameof(NavButtonCheckedBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(NavButtonCheckedBackground), e)));

        public SolidColorBrush NavButtonCheckedForeground
        {
            get { return GetValue(NavButtonCheckedForegroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonCheckedForegroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonCheckedForegroundProperty =
              DependencyProperty.Register(nameof(NavButtonCheckedForeground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(NavButtonCheckedForeground), e)));

        public SolidColorBrush NavButtonPressedBackground
        {
            get { return GetValue(NavButtonPressedBackgroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonPressedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonPressedBackgroundProperty =
              DependencyProperty.Register(nameof(NavButtonPressedBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(NavButtonPressedBackground), e)));

        public SolidColorBrush NavButtonHoverBackground
        {
            get { return GetValue(NavButtonHoverBackgroundProperty) as SolidColorBrush; }
            set { SetValue(NavButtonHoverBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonHoverBackgroundProperty =
              DependencyProperty.Register(nameof(NavButtonHoverBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(NavButtonHoverBackground), e)));

        #endregion

        #region Properties

        public HamburgerButtonInfo Selected
        {
            get { return GetValue(SelectedProperty) as HamburgerButtonInfo; }
            set
            {
                HamburgerButtonInfo oldValue = Selected;
                if ((value?.Equals(oldValue) ?? false))
                    value.IsChecked = (value.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);
                SetValue(SelectedProperty, value);
                SelectedChanged?.Invoke(this, new ChangedEventArgs<HamburgerButtonInfo>(oldValue, value));
            }
        }
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register(nameof(Selected), typeof(HamburgerButtonInfo),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    Changed(nameof(Selected), e);

                    (d as HamburgerMenu)._insideOperation = true;
                    try
                    {
                        (d as HamburgerMenu).SetSelected((HamburgerButtonInfo)e.OldValue, (HamburgerButtonInfo)e.NewValue);
                    }
                    catch (Exception ex)
                    {
                        DebugWrite($"Catch Ex.Message: {ex.Message}", caller: "SelectedPropertyChanged");
                    }
                    finally
                    {
                        (d as HamburgerMenu)._insideOperation = false;
                    }
                }));
        private void SetSelected(HamburgerButtonInfo previous, HamburgerButtonInfo value)
        {
            DebugWrite($"OldValue: {previous}, NewValue: {value}");

            // do not remove this if statement
            //// this is the fix for #410 (click twice)
            if (previous != null)
                IsOpen = (DisplayMode == SplitViewDisplayMode.CompactInline && IsOpen);

            // undo previous
            if (previous?.IsChecked ?? true && previous != value)
            {
                previous?.RaiseUnselected();
            }

            // reset all, except selected
            _navButtons.Where(x => x.Value != value)
                .ForEach(x => { x.Value.IsChecked = false; });

            // navigate only when all navigation buttons have been loaded
            if (_areNavButtonsLoaded && value?.PageType != null)
            {
                if (NavigationService.Navigate(value.PageType, value?.PageParameter, value?.NavigationTransitionInfo))
                {
                    IsOpen = (DisplayMode == SplitViewDisplayMode.CompactInline && IsOpen);
                    if (value.ClearHistory)
                        NavigationService.ClearHistory();
                }
                else if (NavigationService.CurrentPageType == value.PageType
                     && (NavigationService.CurrentPageParam ?? string.Empty) == (value.PageParameter ?? string.Empty))
                {
                    if (value.ClearHistory)
                        NavigationService.ClearHistory();
                }
                else if (NavigationService.CurrentPageType == value.PageType)
                {
                    // just check it
                }
                else
                {
                    return;
                }
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
                DebugWrite($"Value: {value}");

                var open = ShellSplitView.IsPaneOpen;
                if (open == value)
                    return;
                SetValue(IsOpenProperty, value);
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
                    else if (ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactInline && ShellSplitView.IsPaneOpen)
                        ShellSplitView.IsPaneOpen = false;
                }
            }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool),
                typeof(HamburgerMenu), new PropertyMetadata(false,
                    (d, e) =>
                    {
                        Changed(nameof(IsOpen), e);
                        (d as HamburgerMenu).IsOpen = (bool)e.NewValue;
                    }));

        public ObservableCollection<HamburgerButtonInfo> PrimaryButtons
        {
            get
            {
                var PrimaryButtons = (ObservableCollection<HamburgerButtonInfo>)base.GetValue(PrimaryButtonsProperty);
                if (PrimaryButtons == null)
                    SetValue(PrimaryButtonsProperty, PrimaryButtons = new ObservableCollection<HamburgerButtonInfo>());
                return PrimaryButtons;
            }
            set { SetValue(PrimaryButtonsProperty, value); }
        }
        public static readonly DependencyProperty PrimaryButtonsProperty =
            DependencyProperty.Register(nameof(PrimaryButtons), typeof(ObservableCollection<HamburgerButtonInfo>),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(PrimaryButtons), e)));

        private INavigationService _navigationService;
        public INavigationService NavigationService
        {
            get { return _navigationService; }
            set
            {
                DebugWrite($"Value: {value}");

                _navigationService = value;
                ShellSplitView.Content = value.FrameFacade.Frame;

                // Test if there is a splash showing, this is the case if there is no content
                // and if there is a splash factorydefined in the bootstrapper, if true
                // then we want to show the content full screen until the frame loads
                if (_navigationService.FrameFacade.BackStackDepth == 0
                    && BootStrapper.Current.SplashFactory != null
                    && BootStrapper.Current.OriginalActivatedArgs.PreviousExecutionState != Windows.ApplicationModel.Activation.ApplicationExecutionState.Terminated)
                {
                    var once = false;
                    IsFullScreen = true;
                    value.FrameFacade.Navigated += (s, e) =>
                    {
                        if (!once)
                        {
                            once = true;
                            IsFullScreen = false;
                        }
                    };
                }

                UpdateFullScreen();

                NavigationService.AfterRestoreSavedNavigation += (s, e) => HighlightCorrectButton();
                NavigationService.FrameFacade.Navigated += (s, e) => HighlightCorrectButton(e.PageType, e.Parameter);
            }
        }

        /// <summary>
        /// When IsFullScreen is true, the content is displayed on top of the SplitView and the SplitView is
        /// not visible. Even as the user navigates (if possible) the SplitView remains hidden until 
        /// IsFullScreen is set to false. 
        /// </summary>
        /// <remarks>
        /// The original intent for this property was to allow the splash screen to be visible while the
        /// remaining content loaded duing app start. In Minimal (Shell), this is still used for this purpose,
        /// but many developers also leverage this property to view media full screen and similar use cases. 
        /// </remarks>
        public bool IsFullScreen
        {
            get { return (bool)GetValue(IsFullScreenProperty); }
            set { SetValue(IsFullScreenProperty, value); }
        }
        public static readonly DependencyProperty IsFullScreenProperty =
            DependencyProperty.Register(nameof(IsFullScreen), typeof(bool),
                typeof(HamburgerMenu), new PropertyMetadata(false, (d, e) =>
                {
                    Changed(nameof(IsFullScreen), e);
                    (d as HamburgerMenu).UpdateFullScreen();
                }));
        private void UpdateFullScreen(bool? manual = null)
        {
            DebugWrite($"Mavnual: {manual}, IsFullScreen: {IsFullScreen}");

            var frame = NavigationService?.FrameFacade?.Frame;
            if (manual ?? IsFullScreen)
            {
                ShellSplitView.IsHitTestVisible = ShellSplitView.IsEnabled = false;
                ShellSplitView.Content = null;
                if (RootGrid.Children.Contains(ShellSplitView))
                    RootGrid.Children.Remove(ShellSplitView);
                if (!RootGrid.Children.Contains(frame) && frame != null)
                    RootGrid.Children.Add(frame);
            }
            else
            {
                ShellSplitView.IsHitTestVisible = ShellSplitView.IsEnabled = true;
                if (!RootGrid.Children.Contains(ShellSplitView))
                    RootGrid.Children.Add(ShellSplitView);
                if (RootGrid.Children.Contains(frame) && frame != null)
                    RootGrid.Children.Remove(frame);
                ShellSplitView.Content = frame;
            }
        }

        /// <summary>
        /// SecondaryButtons are the button at the bottom of the HamburgerMenu
        /// </summary>
        public ObservableCollection<HamburgerButtonInfo> SecondaryButtons
        {
            get
            {
                var SecondaryButtons = (ObservableCollection<HamburgerButtonInfo>)base.GetValue(SecondaryButtonsProperty);
                if (SecondaryButtons == null)
                    SetValue(SecondaryButtonsProperty, SecondaryButtons = new ObservableCollection<HamburgerButtonInfo>());
                return SecondaryButtons;
            }
            set { SetValue(SecondaryButtonsProperty, value); }
        }
        public static readonly DependencyProperty SecondaryButtonsProperty =
            DependencyProperty.Register(nameof(SecondaryButtons), typeof(ObservableCollection<HamburgerButtonInfo>),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(SecondaryButtons), e)));

        /// <summary>
        /// PaneWidth indicates the width of the Pane when it is open. The width of the Pane
        /// when it is closed is hard-coded to 48 pixels. 
        /// </summary>
        /// <remarks>
        /// The reason the closed width of the pane is hard-coded to 48 pixels is because this
        /// matches the closed width of the MSN News app, after which we modeled this control.
        /// </remarks>
        public double PaneWidth
        {
            get { return (double)GetValue(PaneWidthProperty); }
            set { SetValue(PaneWidthProperty, value); }
        }
        public static readonly DependencyProperty PaneWidthProperty =
            DependencyProperty.Register(nameof(PaneWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata(220d, (d, e) => Changed(nameof(PaneWidth), e)));

        /// <summary>
        /// The Panel border thickness is intended to be the border between between the open
        /// pane and the page content. This is particularly valuable if your menu background
        /// and page background colors are similar in color. You can always set this to 0.
        /// </summary>
        public Thickness PaneBorderThickness
        {
            get { return (Thickness)GetValue(PaneBorderThicknessProperty); }
            set { SetValue(PaneBorderThicknessProperty, value); }
        }
        public static readonly DependencyProperty PaneBorderThicknessProperty =
            DependencyProperty.Register(nameof(PaneBorderThickness), typeof(Thickness),
                typeof(HamburgerMenu), new PropertyMetadata(new Thickness(0, 0, 1, 0), (d, e) => Changed(nameof(PaneBorderThickness), e)));

        /// <summary>
        /// TODO:
        /// </summary>
        public UIElement HeaderContent
        {
            get { return (UIElement)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register(nameof(HeaderContent), typeof(UIElement),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => Changed(nameof(HeaderContent), e)));

        #endregion

        Dictionary<RadioButton, HamburgerButtonInfo> _navButtons = new Dictionary<RadioButton, HamburgerButtonInfo>();
        void NavButton_Loaded(object sender, RoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            // add this radio to the list
            var r = sender as RadioButton;
            var i = r.DataContext as HamburgerButtonInfo;
            if (!_navButtons.ContainsKey(r))
            {
                _navButtons.Add(r, i);
                if (!_areNavButtonsLoaded)
                {
                    _navButtonsLoadedCounter++;
                    if (_navButtonsLoadedCounter >= NavButtonCount)
                        _areNavButtonsLoaded = true;
                }
            }
            HighlightCorrectButton();
        }

        private void NavButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            var radio = sender as RadioButton;
            var info = radio.DataContext as HamburgerButtonInfo;
            info.RaiseTapped(e);

            // do not bubble to SplitView
            e.Handled = true;
        }

        private void NavButton_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            var radio = sender as RadioButton;
            var info = radio.DataContext as HamburgerButtonInfo;
            info.RaiseRightTapped(e);

            // do not bubble to SplitView
            e.Handled = true;
        }

        private void NavButton_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            var radio = sender as RadioButton;
            var info = radio.DataContext as HamburgerButtonInfo;
            info.RaiseHolding(e);

            e.Handled = true;
        }

        StackPanel _SecondaryButtonStackPanel;
        private void SecondaryButtonStackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            DebugWrite();

            _SecondaryButtonStackPanel = sender as StackPanel;
        }

        private int NavButtonCount
        {
            get { return PrimaryButtons.Count + SecondaryButtons.Count; }
        }
        private bool _areNavButtonsLoaded = false;
        private int _navButtonsLoadedCounter = 0;

        bool _insideOperation = false;

        private void NavButtonChecked(object sender, RoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            if (_insideOperation)
                return;
            else
                _insideOperation = true;

            try
            {
                var t = sender as ToggleButton;
                var i = t.DataContext as HamburgerButtonInfo;
                t.IsChecked = (i.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);

                if (t.IsChecked ?? true)
                    HighlightCorrectButton();
                t.IsChecked = Equals(i, Selected);
                if (t.IsChecked ?? true)
                    i.RaiseChecked(e);
            }
            finally
            {
                _insideOperation = false;
            }
        }

        private void NavButtonUnchecked(object sender, RoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            if (_insideOperation)
                return;
            else
                _insideOperation = true;

            try
            {
                var t = sender as ToggleButton;
                var i = t.DataContext as HamburgerButtonInfo;

                if (t.FocusState != FocusState.Unfocused)
                {
                    // prevent un-select
                    t.IsChecked = (i.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);
                    IsOpen = false;
                    return;
                }

                i.RaiseUnchecked(e);
                HighlightCorrectButton();
            }
            finally
            {
                _insideOperation = false;
            }
        }

        #region  OpenClose

        [Flags]
        public enum OpenCloseModes { None = 1, Auto = 2, Tap = 4, Swipe = 5 }

        public OpenCloseModes OpenCloseMode
        {
            get { return (OpenCloseModes)GetValue(OpenCloseModeProperty); }
            set { SetValue(OpenCloseModeProperty, value); }
        }
        public static readonly DependencyProperty OpenCloseModeProperty =
            DependencyProperty.Register(nameof(OpenCloseMode), typeof(OpenCloseModes),
                typeof(HamburgerMenu), new PropertyMetadata(OpenCloseModes.Auto));

        private void PaneContent_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DebugWrite($"OpenCloseMode {OpenCloseMode}");

            if (OpenCloseMode.HasFlag(OpenCloseModes.None))
                return;
            else if (OpenCloseMode.HasFlag(OpenCloseModes.Auto))
            {
                switch (e.PointerDeviceType)
                {
                    case Windows.Devices.Input.PointerDeviceType.Touch:
                        return;
                }
            }
            else if (OpenCloseMode.HasFlag(OpenCloseModes.Tap))
                return;

            HamburgerCommand.Execute(null);
        }

        private void PaneContent_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            DebugWrite($"OpenCloseMode {OpenCloseMode}");

            if (OpenCloseMode.HasFlag(OpenCloseModes.None))
                return;
            else if (OpenCloseMode.HasFlag(OpenCloseModes.Auto))
            {
                switch (e.PointerDeviceType)
                {
                    case Windows.Devices.Input.PointerDeviceType.Pen:
                    case Windows.Devices.Input.PointerDeviceType.Mouse:
                        return;
                }
            }
            else if (!OpenCloseMode.HasFlag(OpenCloseModes.Swipe))
                return;

            var threhold = 24;
            var delta = e.Cumulative.Translation.X;
            if (delta < -threhold)
                IsOpen = false;
            else if (delta > threhold)
                IsOpen = true;
        }

        #endregion
    }
}
