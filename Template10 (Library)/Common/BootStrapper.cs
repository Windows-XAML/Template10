﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Template10.Services.PopupService;
using Template10.Utils;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Template10.Services.ViewService;

namespace Template10.Common
{
    public abstract class BootStrapper : Application, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Set<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (!object.Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        protected void RaisePropertyChanged([CallerMemberName] String propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        /// <summary>
        /// If a developer overrides this method, the developer can resolve DataContext or unwrap DataContext 
        /// available for the Page object when using a MVVM pattern that relies on a wrapped/porxy around ViewModels
        /// </summary>
        public virtual INavigable ResolveForPage(Page page, NavigationService navigationService) => null;

        public static new BootStrapper Current { get; private set; }

        public StateItems SessionState { get; set; } = new StateItems();

        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"BootStrapper.{caller}");

        #endregion

        public BootStrapper()
        {
            DebugWrite("base.Constructor");

            Current = this;
            Resuming += HandleResuming;
            Suspending += HandleSuspending;
        }

        private void Loaded()
        {
            DebugWrite();

            // Hook up keyboard and mouse Back handler
            var KeyboardService = Services.KeyboardService.KeyboardService.Instance;
            KeyboardService.AfterBackGesture = () =>
            {
                DebugWrite(caller: nameof(KeyboardService.AfterBackGesture));

                var handled = false;
                RaiseBackRequested(ref handled);
            };

            KeyboardService.AfterForwardGesture = () =>
            {
                DebugWrite(caller: nameof(KeyboardService.AfterForwardGesture));

                RaiseForwardRequested();
            };

            // Hook up the default Back handler
            SystemNavigationManager.GetForCurrentView().BackRequested += BackHandler;
        }

        public event EventHandler<WindowCreatedEventArgs> WindowCreated;
        protected sealed override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            DebugWrite();
            //should be called to initialize and set new SynchronizationContext
            if (!WindowWrapper.ActiveWrappers.Any())
                Loaded();
            // handle window
            var window = new WindowWrapper(args.Window);
            ViewService.OnWindowCreated();
            WindowCreated?.Invoke(this, args);
            base.OnWindowCreated(args);           
        }

        #region properties

        public INavigationService NavigationService => WindowWrapper.Current().NavigationServices.FirstOrDefault();

        /// <summary>
        /// The SplashFactory is a Func that returns an instantiated Splash view.
        /// Template 10 will automatically inject this visual before loading the app.
        /// </summary>
        public Func<SplashScreen, UserControl> SplashFactory { get; protected set; }

        /// <summary>
        /// CacheMaxDuration indicates the maximum TimeSpan for which cache data
        /// will be preserved. If Template 10 determines cache data is older than
        /// the specified MaxDuration it will automatically be cleared during start.
        /// </summary>
        public TimeSpan CacheMaxDuration { get; set; } = TimeSpan.MaxValue;
        private const string CacheDateKey = "Setting-Cache-Date";

        /// <summary>
        /// ShowShellBackButton is used to show or hide the shell-drawn back button that
        /// is new to Windows 10. A developer can do this manually, but using this property
        /// is important during navigation because Template 10 manages the visibility
        /// of the shell-drawn back button at that time.
        /// </summary>
        public bool ShowShellBackButton { get; set; } = true;

        public bool ForceShowShellBackButton { get; set; } = false;

        #endregion

        #region activated

        // it is the intent of Template 10 to no longer require Launched/Activated overrides, only OnStartAsync()

        protected override sealed async void OnActivated(IActivatedEventArgs e) { DebugWrite(); await InternalActivatedAsync(e).ConfigureAwait(false); }
        protected override sealed async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args).ConfigureAwait(false); }
        protected override sealed async void OnFileActivated(FileActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args).ConfigureAwait(false); }
        protected override sealed async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args).ConfigureAwait(false); }
        protected override sealed async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args).ConfigureAwait(false); }
        protected override sealed async void OnSearchActivated(SearchActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args).ConfigureAwait(false); }
        protected override sealed async void OnShareTargetActivated(ShareTargetActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args).ConfigureAwait(false); }

        public IActivatedEventArgs OriginalActivatedArgs { get; private set; }

        /// <summary>
        /// This handles all the prelimimary stuff unique to Activated before calling OnStartAsync()
        /// This is private because it is a specialized prelude to OnStartAsync().
        /// OnStartAsync will not be called if state restore is determined.
        /// </summary>
        private async Task InternalActivatedAsync(IActivatedEventArgs e)
        {
            DebugWrite();

            OriginalActivatedArgs = e;

            // sometimes activate requires a frame to be built
            if (Window.Current.Content == null)
            {
                DebugWrite("Calling", caller: nameof(InternalActivatedAsync));
                await InitializeFrameAsync(e);
            }

            // onstart is shared with activate and launch
            await CallOnStartAsync(true, StartKind.Activate);

            // ensure active (this will hide any custom splashscreen)
            ActivateWindow(ActivateWindowSources.Activating);
        }

        #endregion

        #region launch

        // it is the intent of Template 10 to no longer require Launched/Activated overrides, only OnStartAsync()

        protected sealed override async void OnLaunched(LaunchActivatedEventArgs e) { DebugWrite(); await InternalLaunchAsync(e).ConfigureAwait(false); }

        /// <summary>
        /// This handles all the preliminary stuff unique to Launched before calling OnStartAsync().
        /// This is private because it is a specialized prelude to OnStartAsync().
        /// OnStartAsync will not be called if state restore is determined
        /// </summary>
        private async Task InternalLaunchAsync(ILaunchActivatedEventArgs e)
        {
            DebugWrite($"Previous:{e.PreviousExecutionState.ToString()}");

            OriginalActivatedArgs = e;

            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                try
                {
                    await InitializeFrameAsync(e);
                }
                catch (Exception)
                {
                    // nothing
                }
            }

            // okay, now handle launch
            bool restored = false;
            switch (e.PreviousExecutionState)
            {
                case ApplicationExecutionState.Suspended:
                case ApplicationExecutionState.Terminated:
                    {
                        OnResuming(this, null, AppExecutionState.Terminated);

                        /*
                            Restore state if you need to/can do.
                            Remember that only the primary tile or when user has
                            switched to the app (for instance via the task switcher)
                            should restore. (this includes toast with no data payload)
                            The rest are already providing a nav path.

                            In the event that the cache has expired, attempting to restore
                            from state will fail because of missing values. 
                            This is okay & by design.
                        */

                        
                        if (EnableAutoRestoreAfterTerminated)
                        {
                            var launchedEvent = e as ILaunchActivatedEventArgs;

                            if(DetermineStartCause(e) == AdditionalKinds.Primary || launchedEvent?.TileId == "")
                            {
                                restored = await NavigationService.RestoreSavedNavigationAsync();
                                DebugWrite($"{nameof(restored)}:{restored}", caller: nameof(NavigationService.RestoreSavedNavigationAsync));
                            }
                        }
                        break;
                    }
                case ApplicationExecutionState.ClosedByUser:
                case ApplicationExecutionState.NotRunning:
                default:
                    break;
            }

            // handle pre-launch
            if ((e as LaunchActivatedEventArgs)?.PrelaunchActivated ?? false)
            {
                var runOnStartAsync = false;
                _HasOnPrelaunchAsync = true;
                await OnPrelaunchAsync(e, out runOnStartAsync);
                if (!runOnStartAsync)
                    return;
            }

            if (!restored)
            {
                var kind = e.PreviousExecutionState == ApplicationExecutionState.Running ? StartKind.Activate : StartKind.Launch;
                await CallOnStartAsync(true, kind);
            }

            ActivateWindow(ActivateWindowSources.Launching);
        }

        public bool EnableAutoRestoreAfterTerminated { get; set; } = true;

        private void BackHandler(object sender, BackRequestedEventArgs args)
        {
            DebugWrite();

            var handled = false;
            if (ApiInformation.IsApiContractPresent(nameof(Windows.Phone.PhoneContract), 1, 0))
            {
                if (NavigationService?.CanGoBack == true)
                {
                    handled = true;
                }
            }
            else
            {
                handled = (NavigationService?.CanGoBack == false);
            }

            RaiseBackRequested(ref handled);
            args.Handled = handled;
        }

        #endregion

        /// <summary>
        /// Default Hardware/Shell Back handler overrides standard Back behavior 
        /// that navigates to previous app in the app stack to instead cause a backward page navigation.
        /// Views or Viewodels can override this behavior by handling the BackRequested 
        /// event and setting the Handled property of the BackRequestedEventArgs to true.
        /// </summary>
        private void RaiseBackRequested(ref bool handled)
        {
            DebugWrite();

            var args = new HandledEventArgs();
            BackRequested?.Invoke(null, args);
            if (handled = args.Handled)
                return;
            foreach (var frame in WindowWrapper.Current().NavigationServices.Select(x => x.FrameFacade).Reverse())
            {
                frame.RaiseBackRequested(args);
                if (handled = args.Handled)
                    return;
            }
            NavigationService.GoBack();
        }

        // this event precedes the in-frame event by the same name
        public static event EventHandler<HandledEventArgs> BackRequested;

        private void RaiseForwardRequested()
        {
            DebugWrite();

            var args = new HandledEventArgs();
            ForwardRequested?.Invoke(null, args);
            if (args.Handled)
                return;
            foreach (var frame in WindowWrapper.Current().NavigationServices.Select(x => x.FrameFacade))
            {
                frame.RaiseForwardRequested(args);
                if (args.Handled)
                    return;
            }
            NavigationService.GoForward();
        }

        public void UpdateShellBackButton()
        {
            DebugWrite();

            // show the shell back only if there is anywhere to go in the default frame
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                (ShowShellBackButton && (NavigationService.CanGoBack || ForceShowShellBackButton))
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
            ShellBackButtonUpdated?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ShellBackButtonUpdated;

        // this event precedes the in-frame event by the same name
        public static event EventHandler<HandledEventArgs> ForwardRequested;

        #region overrides

        public enum StartKind { Launch, Activate }

        /// <summary>
        /// Prelaunch may never occur. However, it's possible that it will. It is a Windows mechanism
        /// to launch apps in the background and quickly suspend them. Because of this, developers need to
        /// handle Prelaunch scenarios if their typical launch is expensive or requires user interaction.
        /// </summary>
        /// <param name="args">IActivatedEventArgs from startup</param>
        /// <param name="runOnStartAsync">A developer can force the typical startup pipeline. Default should be false.</param>
        /// <remarks>
        /// For Prelaunch Template 10 does not continue the typical startup pipeline by default. 
        /// OnActivated will occur if the application has been prelaunched.
        /// </remarks>
        public virtual Task OnPrelaunchAsync(IActivatedEventArgs args, out bool runOnStartAsync)
        {
            DebugWrite("Virtual");

            runOnStartAsync = false;
            return Task.CompletedTask;
        }

        /// <summary>
        /// This indicates if OnPreLaunch has EVER been called.
        /// </summary>
        bool _HasOnPrelaunchAsync = false;

        /// <summary>
        /// OnStartAsync is the one-stop-show override to handle when your app starts
        /// Template 10 will not call OnStartAsync if the app is restored from state.
        /// An app restores from state when the app was suspended and then terminated (PreviousExecutionState terminated).
        /// </summary>
        public abstract Task OnStartAsync(StartKind startKind, IActivatedEventArgs args);

        /// <summary>
        /// This indicates if OnStartAsync has EVER been called
        /// </summary>
        bool _HasOnStartAsync = false;

        /// <summary>
        /// OnInitializeAsync is where your app will do must-have up-front operations
        /// OnInitializeAsync will be called even if the application is restoring from state.
        /// An app restores from state when the app was suspended and then terminated (PreviousExecutionState terminated).
        /// </summary>
        public virtual Task OnInitializeAsync(IActivatedEventArgs args)
        {
            DebugWrite($"Virtual {nameof(IActivatedEventArgs)}:{args.Kind}");

            return Task.CompletedTask;
        }

        /// <summary>
        /// This indicates if OnInitAsync has EVER been called
        /// </summary>
        bool _HasOnInitializeAsync = false;

        /// <summary>
        /// OnSuspendingAsync will be called when the application is suspending, but this override
        /// should only be used by applications that have application-level operations that must
        /// be completed during suspension. 
        /// Using OnSuspendingAsync is a little better than handling the Suspending event manually
        /// because the asunc operations are in a single, global deferral created when the suspension
        /// begins and completed automatically when the last viewmodel has been called (including this method).
        /// </summary>
        public virtual Task OnSuspendingAsync(object s, SuspendingEventArgs e, bool prelaunchActivated)
        {
            DebugWrite($"Virtual {nameof(SuspendingEventArgs)}:{e.SuspendingOperation} {nameof(prelaunchActivated)}:{prelaunchActivated}");

            return Task.CompletedTask;
        }

        public enum AppExecutionState { Suspended, Terminated, Prelaunch }

        /// <summary>
        /// The application is returning from a suspend state of some kind.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="previousExecutionState"></param>
        /// <remarks>
        /// previousExecutionState can be Terminated, which typically does not raise OnResume.
        /// This is important because the resume model changes a little in Mobile.
        /// </remarks>
        public virtual void OnResuming(object s, object e, AppExecutionState previousExecutionState)
        {
            DebugWrite($"Virtual, {nameof(previousExecutionState)}:{previousExecutionState}");
        }

        #endregion

        #region pipeline

        /// <summary>
        /// Creates a new Frame and adds the resulting NavigationService to the 
        /// WindowWrapper collection. In addition, it optionally will setup the 
        /// shell back button to react to the nav of the Frame.
        /// A developer should call this when creating a new/secondary frame.
        /// The shell back button should only be setup one time.
        /// </summary>
        public INavigationService NavigationServiceFactory(BackButton backButton, ExistingContent existingContent)
        {
            DebugWrite($"{nameof(backButton)}:{backButton} {nameof(ExistingContent)}:{existingContent}");

            return NavigationServiceFactory(backButton, existingContent, new Frame());
        }

        /// <summary>
        /// Creates the NavigationService instance for given Frame.
        /// </summary>
        protected virtual INavigationService CreateNavigationService(Frame frame)
        {
            DebugWrite($"Frame:{frame}");

            return new NavigationService(frame);
        }

        /// <summary>
        /// Creates a new NavigationService from the gived Frame to the 
        /// WindowWrapper collection. In addition, it optionally will setup the 
        /// shell back button to react to the nav of the Frame.
        /// A developer should call this when creating a new/secondary frame.
        /// The shell back button should only be setup one time.
        /// </summary>
        public INavigationService NavigationServiceFactory(BackButton backButton, ExistingContent existingContent, Frame frame)
        {
            DebugWrite($"{nameof(backButton)}:{backButton} {nameof(existingContent)}:{existingContent} {nameof(frame)}:{frame}");

            frame.Content = (existingContent == ExistingContent.Include) ? Window.Current.Content : null;

            // if the service already exists for this frame, use the existing one.
            foreach (var nav in WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices))
            {
                if (nav.FrameFacade.Frame.Equals(frame))
                    return nav as INavigationService;
            }

            var navigationService = CreateNavigationService(frame);
            navigationService.FrameFacade.BackButtonHandling = backButton;
            WindowWrapper.Current().NavigationServices.Add(navigationService);

            if (backButton == BackButton.Attach)
            {
                // TODO: unattach others

                // update shell back when backstack changes
                // only the default frame in this case because secondary should not dismiss the app
                frame.RegisterPropertyChangedCallback(Frame.BackStackDepthProperty, (s, args) => UpdateShellBackButton());

                // update shell back when navigation occurs
                // only the default frame in this case because secondary should not dismiss the app
                frame.Navigated += (s, args) => UpdateShellBackButton();
            }

            // this is always okay to check, default or not
            // expire any state (based on expiry)
            DateTime cacheDate;
            // default the cache age to very fresh if not known
            var otherwise = DateTime.MinValue.ToString();
            if (DateTime.TryParse(navigationService.FrameFacade.GetFrameState(CacheDateKey, otherwise), out cacheDate))
            {
                var cacheAge = DateTime.Now.Subtract(cacheDate);
                if (cacheAge >= CacheMaxDuration)
                {
                    // clear state in every nav service in every view
                    foreach (var nav in WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices))
                    {
                        nav.FrameFacade.ClearFrameState();
                    }
                }
            }
            else
            {
                // no date, that's okay
            }

            return navigationService;
        }

        public enum States { Starting, Splashing, ShowingContent }
        public States CurrentState { get; set; } = States.Starting;

        private async Task InitializeFrameAsync(IActivatedEventArgs e)
        {
            /*
                InitializeFrameAsync creates a default Frame preceeded by the optional 
                splash screen, then OnInitialzieAsync, then the new frame (if necessary).
                This is private because there's no reason for the developer to call this.
            */

            DebugWrite($"{nameof(IActivatedEventArgs)}:{e.Kind}");

            ShowSplashScreen(e);
            await CallOnInitializeAsync(true, e);
            SetupCustomTitleBar();

            // if there's custom content then there's nothing to do
            if (CurrentState == States.Splashing)
            {
                Window.Current.Content = CreateRootElement(e);
            }
            else if (Window.Current.Content == null)
            {
                Window.Current.Content = CreateRootElement(e);
            }
            else
            {
                // nothing: custom content
            }
        }

        #endregion

        public enum ActivateWindowSources { Launching, Activating, SplashScreen, Resuming }
        /// <summary>
        /// Override this method only if you (the developer) wants to programmatically
        /// control the means by which and when the Core Window is activated by Template 10.
        /// One scenario might be a delayed activation for Splash Screen.
        /// </summary>
        /// <param name="source">Reason for the call from Template 10</param>
        private void ActivateWindow(ActivateWindowSources source)
        {
            DebugWrite($"source:{source}");

            if (source != ActivateWindowSources.SplashScreen)
            {
                if (CurrentState == States.Splashing)
                    SplashScreenPopup?.Hide();
                CurrentState = States.ShowingContent;
            }

            Window.Current.Activate();
        }

        #region Workers

        /// <summary>
        ///  By default, Template 10 will setup the root element to be a Template 10
        ///  Modal Dialog control. If you desire something different, you can set it here.
        /// </summary>
        public virtual UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var b = Current;
            var frame = new Frame();
            var nav = b.NavigationServiceFactory(BackButton.Attach, ExistingContent.Include, frame);
            return new Controls.ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = nav.Frame
            };
        }

        private void SetupCustomTitleBar()
        {
            /*
                this "unused" bit is very important because of a quirk in ResourceThemes
            */

            try
            {
                if (Application.Current.Resources.ContainsKey("ExtendedSplashBackground"))
                {
                    var unused = Application.Current.Resources["ExtendedSplashBackground"];
                }
            }
            catch { /* this is okay */ }

            // this wonky style of loop is important due to a platform bug
            int count = Application.Current.Resources.Count;
            foreach (var resource in Application.Current.Resources)
            {
                var k = resource.Key;
                if (k == typeof(Controls.CustomTitleBar))
                {
                    var s = resource.Value as Style;
                    var t = new Controls.CustomTitleBar();
                    t.Style = s;
                }
                count--;
                if (count == 0) break;
            }
        }

        private async Task CallOnInitializeAsync(bool canRepeat, IActivatedEventArgs e)
        {
            if (!canRepeat && _HasOnInitializeAsync)
                return;
            _HasOnInitializeAsync = true;
            await OnInitializeAsync(e);
        }

        private async Task CallOnStartAsync(bool canRepeat, StartKind startKind)
        {
            DebugWrite();

            if (!canRepeat && _HasOnStartAsync)
                return;
            _HasOnStartAsync = true;
            await OnStartAsync(startKind, OriginalActivatedArgs);
        }

        internal Popup SplashScreenPopup = null;
        private void ShowSplashScreen(IActivatedEventArgs e)
        {
            if (SplashFactory != null)
            {
                CurrentState = States.Splashing;
                var splash = SplashFactory(e.SplashScreen);
                var service = new PopupService();
                SplashScreenPopup = service.Show(PopupService.PopupSize.FullScreen, splash);
                ActivateWindow(ActivateWindowSources.SplashScreen);
            }
        }

        [Obsolete("Use RootElementFactory.")]
        protected virtual Frame CreateRootFrame(IActivatedEventArgs e)
        {
            DebugWrite($"{nameof(IActivatedEventArgs)}:{e}");

            return new Frame();
        }

        private async void HandleResuming(object sender, object e)
        {
            DebugWrite(caller: nameof(Resuming));

            var args = OriginalActivatedArgs as LaunchActivatedEventArgs;
            if (args?.PrelaunchActivated ?? true)
            {
                OnResuming(sender, e, AppExecutionState.Prelaunch);
                var kind = args?.PreviousExecutionState == ApplicationExecutionState.Running ? StartKind.Activate : StartKind.Launch;
                await CallOnStartAsync(false, kind);
                ActivateWindow(ActivateWindowSources.Resuming);
            }
            else
            {
                OnResuming(sender, e, AppExecutionState.Suspended);
            }
        }

        private async void HandleSuspending(object sender, SuspendingEventArgs e)
        {
            DebugWrite();

            // one, global deferral
            var deferral = e.SuspendingOperation.GetDeferral();
            using (var session = new Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionSession
            {
                Description = this.GetType().ToString(),
                Reason = Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionReason.SavingData
            })
            {
                try
                {
                    //allow only main view NavigationService as others won't be able to use Dispatcher and processing will stuck
                    var services = WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices).Where(x => x.IsInMainView);
                    foreach (INavigationService nav in services)
                    {
                        // date the cache (which marks the date/time it was suspended)
                        nav.FrameFacade.SetFrameState(CacheDateKey, DateTime.Now.ToString());
                        // call view model suspend (OnNavigatedfrom)
                        DebugWrite($"Nav.FrameId:{nav.FrameFacade.FrameId}", caller: nameof(nav.SuspendingAsync));
                        await (nav as INavigationService).GetDispatcherWrapper().DispatchAsync(async () => await nav.SuspendingAsync());
                    }

                    // call system-level suspend
                    DebugWrite($"Calling. OnSuspendingAsync {(OriginalActivatedArgs as LaunchActivatedEventArgs)?.PrelaunchActivated ?? false}", caller: nameof(OnSuspendingAsync));
                    await OnSuspendingAsync(sender, e, (OriginalActivatedArgs as LaunchActivatedEventArgs)?.PrelaunchActivated ?? false);
                }
                catch { /* do nothing */ }
                finally { deferral.Complete(); }
            }
        }

        #endregion

        // The default frame is automatically wrapped in a modal dialog.
        // this is how you access it to set ModalContent or the IsModal property. 
        public Controls.ModalDialog ModalDialog { get { return (Window.Current.Content as Controls.ModalDialog); } }
        public UIElement ModalContent { get { return ModalDialog?.ModalContent; } set { if (ModalDialog != null) ModalDialog.ModalContent = value; } }

        public enum BackButton { Attach, Ignore }

        public enum ExistingContent { Include, Exclude }

        public const string DefaultTileID = "App";

        public enum AdditionalKinds { Primary, Toast, SecondaryTile, Other, JumpListItem }

        /// <summary>
        /// This determines the simplest case for starting. This should handle 80% of common scenarios. 
        /// When Other is returned the developer must determine start manually using IActivatedEventArgs.Kind
        /// </summary>
        public static AdditionalKinds DetermineStartCause(IActivatedEventArgs args)
        {
            DebugWrite($"{nameof(IActivatedEventArgs)}:{args.Kind}");

            if (args is ToastNotificationActivatedEventArgs)
            {
                return AdditionalKinds.Toast;
            }
            var e = args as ILaunchActivatedEventArgs;
            if (e?.TileId == DefaultTileID && string.IsNullOrEmpty(e?.Arguments))
            {
                return AdditionalKinds.Primary;
            }
            else if (e?.TileId == DefaultTileID && !string.IsNullOrEmpty(e?.Arguments))
            {
                return AdditionalKinds.JumpListItem;
            }
            else if (!string.IsNullOrEmpty(e?.TileId) && e?.TileId != DefaultTileID)
            {
                return AdditionalKinds.SecondaryTile;
            }
            else
            {
                return AdditionalKinds.Other;
            }
        }

        private object _PageKeys;
        // T must be a custom Enum
        public Dictionary<T, Type> PageKeys<T>()
            where T : struct, IConvertible
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            if (_PageKeys != null && _PageKeys is Dictionary<T, Type>)
            {
                return _PageKeys as Dictionary<T, Type>;
            }
            return (_PageKeys = new Dictionary<T, Type>()) as Dictionary<T, Type>;
        }
    }
}
