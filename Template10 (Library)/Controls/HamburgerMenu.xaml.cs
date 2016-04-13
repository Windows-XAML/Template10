using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Template10.Common;
using Template10.Services.KeyboardService;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    [ContentProperty(Name = nameof(PrimaryButtons))]
    public sealed partial class HamburgerMenu : UserControl
    {
        const int squareWidth = 48;
        const int squareHeight = 48;
        delegate void PropertyChangeHandlerDelegate(DependencyPropertyChangedEventArgs e);

        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"HamburgerMenu.{caller}");

        #endregion

        public HamburgerMenu()
        {
            DebugWrite();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                InitializeComponent();
            }
            else
            {
                // hamburger menu property changes
                PropertyChangedHandlers.Add(nameof(IsFullScreen), e => FullScreenPropertyChanged((bool?)e.OldValue, (bool?)e.NewValue));
                PropertyChangedHandlers.Add(nameof(Selected), e => SelectedPropertyChanged(e.OldValue as HamburgerButtonInfo, e.NewValue as HamburgerButtonInfo));
                PropertyChangedHandlers.Add(nameof(DisplayMode), e => DisplayModePropertyChanged((SplitViewDisplayMode)e.OldValue, (SplitViewDisplayMode)e.NewValue));
                PropertyChangedHandlers.Add(nameof(HamburgerButtonVisibility), e => HamburgerButtonVisibilityPropertyChanged((Visibility)e.NewValue));
                PropertyChangedHandlers.Add(nameof(IsOpen), e => IsOpenPropertyChanged((bool)e.OldValue, (bool)e.NewValue));
                PropertyChangedHandlers.Add(nameof(NavigationService), e => NavigationServicePropertyChanged(e.OldValue as INavigationService, e.NewValue as INavigationService));
                PropertyChangedHandlers.Add(nameof(AccentColor), e => AccentColorPropertyChanged(e.OldValue as Color?, e.NewValue as Color?));

                // default values;
                PrimaryButtons = new ObservableItemCollection<HamburgerButtonInfo>();
                SecondaryButtons = new ObservableItemCollection<HamburgerButtonInfo>();

                // calling this now, let's handlers wire up before styles apply
                InitializeComponent();

                // control event handlers
                Loaded += HamburgerMenu_Loaded;
                LayoutUpdated += HamburgerMenu_LayoutUpdated;
                KeyboardService.Instance.AfterMenuGesture += () => IsOpen = !IsOpen;
            }

        }

        void HamburgerMenu_Loaded(object sender, RoutedEventArgs args)
        {
            DebugWrite();

            // splitview property changes
            ShellSplitView.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (d, e) => SplitViewIsPaneOpenChanged(e));
            ShellSplitView.RegisterPropertyChangedCallback(SplitView.DisplayModeProperty, (d, e) => SplitViewDisplayModeChanged(e));
        }

        bool _hasLayoutUpdatedOnce;
        void HamburgerMenu_LayoutUpdated(object sender, object e)
        {
            DebugWrite();

            if (!_hasLayoutUpdatedOnce)
            {
                _hasLayoutUpdatedOnce = true;
                SetFullScreen();
            }
        }

        #region property changed handlers

        void SplitViewDisplayModeChanged(DependencyProperty dp) => DisplayMode = ShellSplitView.DisplayMode;

        void SplitViewIsPaneOpenChanged(DependencyProperty dp)
        {
            // this can occur if the user resizes before it loads
            if (_SecondaryButtonStackPanel == null)
            {
                return;
            }

            // secondary layout
            if (SecondaryButtonOrientation.Equals(Orientation.Horizontal) && ShellSplitView.IsPaneOpen)
            {
                _SecondaryButtonStackPanel.Orientation = Orientation.Horizontal;
            }
            else
            {
                _SecondaryButtonStackPanel.Orientation = Orientation.Vertical;
            }

            // overall events
            if (ShellSplitView.IsPaneOpen)
            {
                PaneOpened?.Invoke(ShellSplitView, EventArgs.Empty);
                HamburgerButtonGridWidth = (ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactInline) ? PaneWidth : squareWidth;
            }
            else
            {
                PaneClosed?.Invoke(ShellSplitView, EventArgs.Empty);
            }

            // this will keep the two properties in sync
            if (IsOpen != ShellSplitView.IsPaneOpen)
                IsOpen = ShellSplitView.IsPaneOpen;
        }

        void AccentColorPropertyChanged(Color? previous, Color? value) => RefreshStyles(value);

        void FullScreenPropertyChanged(bool? previous, bool? value) => SetFullScreen(value);

        void IsOpenPropertyChanged(bool previous, bool value)
        {
            // this will keep the two properties in sync
            if (value)
            {
                ShellSplitView.IsPaneOpen = true;
                HamburgerButtonGridWidth = (ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactInline) ? PaneWidth : squareWidth;
            }
            else
            {
                // collapse the window
                if (ShellSplitView.DisplayMode == SplitViewDisplayMode.Overlay && ShellSplitView.IsPaneOpen)
                {
                    ShellSplitView.IsPaneOpen = false;
                }
                else if (ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactOverlay && ShellSplitView.IsPaneOpen)
                {
                    ShellSplitView.IsPaneOpen = false;
                }
                else if (ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactInline && ShellSplitView.IsPaneOpen)
                {
                    ShellSplitView.IsPaneOpen = false;
                }
                HamburgerButtonGridWidth = squareWidth;
            }
        }

        void DisplayModePropertyChanged(SplitViewDisplayMode previous, SplitViewDisplayMode value)
        {
            // this will keep the two properties in sync
            if (ShellSplitView.DisplayMode != value)
            {
                ShellSplitView.DisplayMode = value;
            }
            HamburgerButtonGridWidth = (value == SplitViewDisplayMode.CompactInline) ? PaneWidth : squareWidth;
        }

        void HamburgerButtonVisibilityPropertyChanged(Visibility value) => HamburgerButton.Visibility = value;

        object NavButtonInsideOperationLock = new object();
        async void SelectedPropertyChanged(HamburgerButtonInfo previous, HamburgerButtonInfo value)
        {
            if ((value?.Equals(previous) ?? false))
            {
                value.IsChecked = (value.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);
            }

            SelectedChanged?.Invoke(this, new ChangedEventArgs<HamburgerButtonInfo>(previous, value));

            Monitor.Enter(NavButtonInsideOperationLock);
            try
            {
                await SetSelectedAsync(previous, value);
            }
            catch (Exception ex)
            {
                DebugWrite($"Catch Ex.Message: {ex.Message}", caller: "SelectedPropertyChanged");
            }
            finally
            {
                Monitor.Exit(NavButtonInsideOperationLock);
            }
        }

        void NavigationServicePropertyChanged(INavigationService previous, INavigationService value)
        {
            ShellSplitView.Content = value.FrameFacade.Frame;

            // If splash screen then continue showing until navigated once
            if (value.FrameFacade.BackStackDepth == 0
                && value.Frame.Content != null
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

            SetFullScreen();

            value.AfterRestoreSavedNavigation += (s, e) => HighlightCorrectButton(NavigationService.CurrentPageType, NavigationService.CurrentPageParam);
            value.FrameFacade.Navigated += (s, e) => HighlightCorrectButton(e.PageType, e.Parameter);
        }

        void IsFullScreenPropertyChanged(bool previous, bool value) => SetFullScreen(value);

        #endregion

        internal void HighlightCorrectButton(Type pageType, object pageParam)
        {
            DebugWrite($"PageType: {pageType} PageParam: {pageParam}");

            // match type only
            var buttons = LoadedNavButtons
                .Where(x => Equals(x.Value.PageType, pageType));

            if (pageParam == null)
            {
                pageParam = NavigationService.CurrentPageParam;
            }
            else if (pageParam.ToString().StartsWith("{"))
            {
                try
                {
                    pageParam = NavigationService.FrameFacade.SerializationService.Deserialize(pageParam.ToString());
                }
                catch { }
            }

            // add parameter match
            buttons = buttons
                .Where(x => Equals(x.Value.PageParameter, null) || Equals(x.Value.PageParameter, pageParam));

            var button = buttons.Select(x => x.Value).FirstOrDefault();
            Selected = button;
        }

        async Task SetSelectedAsync(HamburgerButtonInfo previous, HamburgerButtonInfo value)
        {
            DebugWrite($"OldValue: {previous}, NewValue: {value}");

            // pls. do not remove this if statement. this is the fix for #410 (click twice)
            if (previous != null)
            {
                IsOpen = (DisplayMode == SplitViewDisplayMode.CompactInline && IsOpen);
            }

            // signal previous
            if (previous?.IsChecked ?? true && previous != value)
            {
                previous?.RaiseUnselected();
            }

            // reset all, except selected
            var buttons = LoadedNavButtons.Where(x => x.Value != value).Select(x => x.Value);
            buttons.ForEach(x => x.IsChecked = false);

            // navigate only when all navigation buttons have been loaded
            if (AllNavButtonsAreLoaded && value?.PageType != null)
            {
                if (await NavigationService.NavigateAsync(value.PageType, value?.PageParameter, value?.NavigationTransitionInfo))
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
        void SetFullScreen(bool? manual = null)
        {
            if (_hasLayoutUpdatedOnce)
            {
                DebugWrite($"Manual: {manual}, IsFullScreen: {IsFullScreen}");

                var frame = NavigationService?.Frame;
                if (manual ?? IsFullScreen)
                {
                    ShellSplitView.IsHitTestVisible = ShellSplitView.IsEnabled = false;
                    AutomationProperties.SetAccessibilityView(ShellSplitView, Windows.UI.Xaml.Automation.Peers.AccessibilityView.Raw);
                    ShellSplitView.Content = null;
                    if (!RootGrid.Children.Contains(frame) && frame != null)
                    {
                        RootGrid.Children.Add(frame);
                    }
                }
                else
                {
                    ShellSplitView.IsHitTestVisible = ShellSplitView.IsEnabled = true;
                    AutomationProperties.SetAccessibilityView(ShellSplitView, Windows.UI.Xaml.Automation.Peers.AccessibilityView.Control);
                    if (RootGrid.Children.Contains(frame) && frame != null)
                    {
                        RootGrid.Children.Remove(frame);
                    }
                    ShellSplitView.Content = frame;
                }
            }
        }

        StackPanel _SecondaryButtonStackPanel;
        void SecondaryButtonStackPanel_Loaded(object sender, RoutedEventArgs e) => _SecondaryButtonStackPanel = sender as StackPanel;

        #region Nav Buttons

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
            {
                throw new NullReferenceException("CommandParameter is not set");
            }

            if (commandInfo.PageType != null)
            {
                Selected = commandInfo;
            }
        }

        #endregion

        bool AllNavButtonsAreLoaded => LoadedNavButtons.Count >= NavButtonCount;
        readonly Dictionary<RadioButton, HamburgerButtonInfo> LoadedNavButtons = new Dictionary<RadioButton, HamburgerButtonInfo>();

        void NavButton_Loaded(object sender, RoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            AddNavButtonToNavButtons(sender as RadioButton);
        }

        void AddNavButtonToNavButtons(RadioButton button)
        {
            DebugWrite();

            if (LoadedNavButtons.ContainsKey(button)) return;
            var info = button.DataContext as HamburgerButtonInfo;
            LoadedNavButtons.Add(button, info);
            if (AllNavButtonsAreLoaded)
            {
                HighlightCorrectButton(NavigationService.CurrentPageType, NavigationService.CurrentPageParam);
            }
        }

        void NavButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            var radio = sender as RadioButton;
            var info = radio.DataContext as HamburgerButtonInfo;
            info.RaiseTapped(e);
            ExecuteICommand(info);

            // do not bubble to SplitView
            e.Handled = true;
        }

        void ExecuteICommand(HamburgerButtonInfo info)
        {
            ICommand command = info.Command;
            if (command != null)
            {
                var commandParameter = info.CommandParameter;
                if (command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            }
        }

        void NavButton_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            var radio = sender as RadioButton;
            var info = radio.DataContext as HamburgerButtonInfo;
            info.RaiseRightTapped(e);

            // do not bubble to SplitView
            e.Handled = true;
        }

        void NavButton_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            var radio = sender as RadioButton;
            var info = radio.DataContext as HamburgerButtonInfo;
            info.RaiseHolding(e);

            e.Handled = true;
        }

        int NavButtonCount => PrimaryButtons.Count + SecondaryButtons.Count;

        void NavButtonChecked(object sender, RoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            Monitor.Enter(NavButtonInsideOperationLock);
            try
            {
                var radio = sender as ToggleButton;
                var info = radio.DataContext as HamburgerButtonInfo;

                // only toggle buttons can be checked
                radio.IsChecked = (info.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);

                if (radio.IsChecked ?? true) Selected = info;
                radio.IsChecked = Equals(info, Selected);
                if (radio.IsChecked ?? true) info.RaiseChecked(e);
            }
            finally
            {
                Monitor.Exit(NavButtonInsideOperationLock);
            }
        }

        void NavButtonUnchecked(object sender, RoutedEventArgs e)
        {
            DebugWrite($"Info: {(sender as FrameworkElement).DataContext}");

            Monitor.Enter(NavButtonInsideOperationLock);
            try
            {
                var radio = sender as ToggleButton;
                var info = radio.DataContext as HamburgerButtonInfo;

                if (radio.FocusState != FocusState.Unfocused)
                {
                    // prevent un-select
                    radio.IsChecked = (info.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);
                    IsOpen = false;
                    return;
                }

                info.RaiseUnchecked(e);
                HighlightCorrectButton(NavigationService.CurrentPageType, NavigationService.CurrentPageParam);
            }
            finally
            {
                Monitor.Exit(NavButtonInsideOperationLock);
            }
        }

        #endregion

        #region  Touch gesture to OpenClose

        void PaneContent_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DebugWrite($"OpenCloseMode {OpenCloseMode}");

            if (OpenCloseMode.HasFlag(OpenCloseModes.None)) return;
            else if (OpenCloseMode.HasFlag(OpenCloseModes.Auto))
            {
                switch (e.PointerDeviceType)
                {
                    case Windows.Devices.Input.PointerDeviceType.Touch:
                        return;
                }
            }
            else if (OpenCloseMode.HasFlag(OpenCloseModes.Tap)) return;
            HamburgerCommand.Execute(null);
        }

        void PaneContent_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            DebugWrite($"OpenCloseMode {OpenCloseMode}");

            if (OpenCloseMode.HasFlag(OpenCloseModes.None)) return;
            else if (OpenCloseMode.HasFlag(OpenCloseModes.Auto))
            {
                // this is only for touch
                switch (e.PointerDeviceType)
                {
                    case Windows.Devices.Input.PointerDeviceType.Pen:
                    case Windows.Devices.Input.PointerDeviceType.Mouse:
                        return;
                }
            }
            else if (!OpenCloseMode.HasFlag(OpenCloseModes.Swipe)) return;

            var threshold = 24;
            var delta = e.Cumulative.Translation.X;
            if (delta < -threshold) IsOpen = false;
            else if (delta > threshold) IsOpen = true;
        }

        #endregion
    }
}
