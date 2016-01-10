﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    public abstract class BootStrapper : Application
    {
        #region dependency injection

        /// <summary>       
        /// A developer implements this method to return not any type and would need to 
        /// switch on the type param, returnig the correctly inflated type.
        /// </summary>
        /// <remarks>
        /// There are two popular approaches to view-model dependency injection. The first is this approach, 
        /// where a Resolve method exists for the developer to implement. The second is a view-model locator
        /// which puts the injection logic in the form of a property. Both are equally valid.
        /// </remarks>
        public virtual T Resolve<T>(Type type) => default(T);

        /// <summary>
        /// If a developer overrides this method, and leaves the DataContext of a page null, then BootStrapper
        /// will atttempt to fill the DataContext the return value of this method. 
        /// </summary>
        public virtual Services.NavigationService.INavigable ResolveForPage(Type page, NavigationService navigationService) => null;

        #endregion

        public static new BootStrapper Current { get; private set; }

        public StateItems SessionState { get; set; } = new StateItems();

        protected BootStrapper()
        {
            Current = this;
            Resuming += (s, e) => { OnResuming(s, e); };
            Suspending += async (s, e) =>
            {
                // one, global deferral
                var deferral = e.SuspendingOperation.GetDeferral();
                try
                {
                    foreach (var nav in WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices))
                    {
                        // date the cache (which marks the date/time it was suspended)
                        nav.FrameFacade.SetFrameState(CacheDateKey, DateTime.Now.ToString());
                        // call view model suspend (OnNavigatedfrom)
                        await nav.SuspendingAsync();
                    }
                    // call system-level suspend
                    await OnSuspendingAsync(s, e);
                }
                catch { }
                finally { deferral.Complete(); }
            };
        }

        #region properties

        public Services.NavigationService.INavigationService NavigationService => WindowWrapper.Current().NavigationServices.FirstOrDefault();

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

        #endregion

        #region activated

        // it is the intent of Template 10 to no longer require Launched/Activated overrides, only OnStartAsync()

        protected override sealed async void OnActivated(IActivatedEventArgs e) { await InternalActivatedAsync(e); }
        protected override sealed async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args) { await InternalActivatedAsync(args); }
        protected override sealed async void OnFileActivated(FileActivatedEventArgs args) { await InternalActivatedAsync(args); }
        protected override sealed async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args) { await InternalActivatedAsync(args); }
        protected override sealed async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args) { await InternalActivatedAsync(args); }
        protected override sealed async void OnSearchActivated(SearchActivatedEventArgs args) { await InternalActivatedAsync(args); }
        protected override sealed async void OnShareTargetActivated(ShareTargetActivatedEventArgs args) { await InternalActivatedAsync(args); }

        IActivatedEventArgs OriginalActivatedArgs;

        /// <summary>
        /// This handles all the prelimimary stuff unique to Activated before calling OnStartAsync()
        /// This is private because it is a specialized prelude to OnStartAsync().
        /// OnStartAsync will not be called if state restore is determined.
        /// </summary>
        private async Task InternalActivatedAsync(IActivatedEventArgs e)
        {
            OriginalActivatedArgs = e;

            // sometimes activate requires a frame to be built
            if (Window.Current.Content == null)
            {
                await InitializeFrameAsync(e);
            }

            // onstart is shared with activate and launch
            await OnStartAsync(StartKind.Activate, e);

            // ensure active (this will hide any custom splashscreen)
            Window.Current.Activate();
        }

        #endregion

        public event EventHandler<WindowCreatedEventArgs> WindowCreated;
        protected sealed override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            var window = new WindowWrapper(args.Window);
            WindowCreated?.Invoke(this, args);
            base.OnWindowCreated(args);
        }

        #region launch

        // it is the intent of Template 10 to no longer require Launched/Activated overrides, only OnStartAsync()

        protected override sealed void OnLaunched(LaunchActivatedEventArgs e) { InternalLaunchAsync(e); }

        /// <summary>
        /// This handles all the preliminary stuff unique to Launched before calling OnStartAsync().
        /// This is private because it is a specialized prelude to OnStartAsync().
        /// OnStartAsync will not be called if state restore is determined
        /// </summary>
        private async void InternalLaunchAsync(ILaunchActivatedEventArgs e)
        {
            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                await InitializeFrameAsync(e);
            }

            // okay, now handle launch
            switch (e.PreviousExecutionState)
            {
                //case ApplicationExecutionState.ClosedByUser:
                case ApplicationExecutionState.Terminated:
                    {
                        /*
                            Restore state if you need to/can do.
                            Remember that only the primary tile should restore.
                            (this includes toast with no data payload)
                            The rest are already providing a nav path.

                            In the event that the cache has expired, attempting to restore
                            from state will fail because of missing values. 
                            This is okay & by design.
                        */

                        if (DetermineStartCause(e) == AdditionalKinds.Primary)
                        {
                            var restored = NavigationService.RestoreSavedNavigation();
                            if (!restored)
                            {
                                await OnStartAsync(StartKind.Launch, e);
                            }
                        }
                        else
                        {
                            await OnStartAsync(StartKind.Launch, e);
                        }
                        break;
                    }
                case ApplicationExecutionState.ClosedByUser:
                case ApplicationExecutionState.NotRunning:
                    await OnStartAsync(StartKind.Launch, e);
                    break;
                default:
                    await OnStartAsync(StartKind.Launch, e);
                    break;
            }
            SubscribeBackButton();

            // ensure active (this will hide any custom splashscreen)
            Window.Current.Activate();

            // Hook up keyboard and mouse Back handler
            var keyboard = Services.KeyboardService.KeyboardService.Instance;
            keyboard.AfterBackGesture = async() =>
            {//fire and forget
                //the result is no matter
                await RaiseBackRequested(false);
            };

            // Hook up keyboard and mouse Forward handler
            keyboard.AfterForwardGesture = async () =>
            {//fire and forget
               await RaiseForwardRequested();
            };
        }

        private void SubscribeBackButton()
        {
            // Hook up the default Back handler
            SystemNavigationManager.GetForCurrentView().BackRequested -= BackHandler;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackHandler;
        }

        private async void BackHandler(object sender, BackRequestedEventArgs args)
        {
            var handled = false;
            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
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
            //this is always handled
            await RaiseBackRequested(handled);
            args.Handled = true;
        }

        #endregion

        /// <summary>
        /// Default Hardware/Shell Back handler overrides standard Back behavior 
        /// that navigates to previous app in the app stack to instead cause a backward page navigation.
        /// Views or Viewodels can override this behavior by handling the BackRequested 
        /// event and setting the Handled property of the BackRequestedEventArgs to true.
        /// </summary>
        private async Task RaiseBackRequested(bool handled)
        {
            var args = new HandledEventArgs();
            BackRequested?.Invoke(null, args);
            if (handled = args.Handled)
                return; 
            foreach (var frame in WindowWrapper.Current().NavigationServices.Select(x => x.FrameFacade).Reverse())
            {
                await frame.RaiseBackRequested(args);
                if (handled = args.Handled)
                    return;
            }
            NavigationService.GoBack();
        }

        // this event precedes the in-frame event by the same name
        public static event EventHandler<HandledEventArgs> BackRequested;

        private async Task RaiseForwardRequested()
        {
            var args = new HandledEventArgs();
            ForwardRequested?.Invoke(null, args);
            if (args.Handled)
                return;
            foreach (var frame in WindowWrapper.Current().NavigationServices.Select(x => x.FrameFacade))
            {
                await frame.RaiseForwardRequested(args);
                if (args.Handled)
                    return;
            }
            NavigationService.GoForward();
        }

        // this event precedes the in-frame event by the same name
        public static event EventHandler<HandledEventArgs> ForwardRequested;

        #region overrides

        public enum StartKind { Launch, Activate }

        /// <summary>
        /// OnStartAsync is the one-stop-show override to handle when your app starts
        /// Template 10 will not call OnStartAsync if the app is restored from state.
        /// An app restores from state when the app was suspended and then terminated (PreviousExecutionState terminated).
        /// </summary>
        public abstract Task OnStartAsync(StartKind startKind, IActivatedEventArgs args);

        /// <summary>
        /// OnInitializeAsync is where your app will do must-have up-front operations
        /// OnInitializeAsync will be called even if the application is restoring from state.
        /// An app restores from state when the app was suspended and then terminated (PreviousExecutionState terminated).
        /// </summary>
        public virtual async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// OnSuspendingAsync will be called when the application is suspending, but this override
        /// should only be used by applications that have application-level operations that must
        /// be completed during suspension. 
        /// Using OnSuspendingAsync is a little better than handling the Suspending event manually
        /// because the asunc operations are in a single, global deferral created when the suspension
        /// begins and completed automatically when the last viewmodel has been called (including this method).
        /// </summary>
        public virtual async Task OnSuspendingAsync(object s, SuspendingEventArgs e)
        {
            await Task.CompletedTask;
        }

        public virtual void OnResuming(object s, object e) { }

        #endregion

        /// <summary>
        /// InitializeFrameAsync creates a default Frame preceeded by the optional 
        /// splash screen, then OnInitialzieAsync, then the new frame (if necessary).
        /// This is private because there's no reason for the developer to call this.
        /// </summary>
        private async Task InitializeFrameAsync(IActivatedEventArgs e)
        {
            // first show the splash 
            FrameworkElement splash = null;
            if (SplashFactory != null)
            {
                Window.Current.Content = splash = SplashFactory(e.SplashScreen);
                Window.Current.Activate();
            }

            // allow the user to do things, even when restoring
            await OnInitializeAsync(e);

            // setup custom titlebar
            foreach (var resource in Application.Current.Resources
                .Where(x => x.Key.Equals(typeof(Controls.CustomTitleBar))))
            {
                var control = new Controls.CustomTitleBar();
                control.Style = resource.Value as Style;
            }

            // create the default frame only if there's nothing already there
            // if it is not null, by the way, then the developer injected something & they win
            if (Window.Current.Content == null || Window.Current.Content == splash)
            {
                // build the default frame
                var frame = CreateRootFrame(e);
                Window.Current.Content = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include, frame).Frame;
            }
        }

        protected virtual Frame CreateRootFrame(IActivatedEventArgs e)
        {
            return new Frame();
        }

        public enum BackButton { Attach, Ignore }
        public enum ExistingContent { Include, Exclude }

        /// <summary>
        /// Creates a new Frame and adds the resulting NavigationService to the 
        /// WindowWrapper collection. In addition, it optionally will setup the 
        /// shell back button to react to the nav of the Frame.
        /// A developer should call this when creating a new/secondary frame.
        /// The shell back button should only be setup one time.
        /// </summary>
        public Services.NavigationService.INavigationService NavigationServiceFactory(BackButton backButton, ExistingContent existingContent)
        {
            return NavigationServiceFactory(backButton, existingContent, new Frame());
        }

        /// <summary>
        /// Creates the NavigationService instance for given Frame.
        /// </summary>
        protected virtual Services.NavigationService.INavigationService CreateNavigationService(Frame frame)
        {
            return new Services.NavigationService.NavigationService(frame);
        }

        /// <summary>
        /// Creates a new NavigationService from the gived Frame to the 
        /// WindowWrapper collection. In addition, it optionally will setup the 
        /// shell back button to react to the nav of the Frame.
        /// A developer should call this when creating a new/secondary frame.
        /// The shell back button should only be setup one time.
        /// </summary>
        public Services.NavigationService.INavigationService NavigationServiceFactory(BackButton backButton, ExistingContent existingContent, Frame frame)
        {
            // if the service already exists for this frame, use the existing one.
            foreach (var nav in WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices))
            {
                if (nav.Frame.Equals(frame))
                    return nav;
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
                    foreach (var service in WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices))
                    {
                        service.FrameFacade.ClearFrameState();
                    }
                }
            }
            else
            {
                // no date, that's okay
            }

            return navigationService;
        }

        public const string DefaultTileID = "App";

        public bool ForceShowShellBackButton { get; set; } = false;
        public void UpdateShellBackButton()
        {
            // show the shell back only if there is anywhere to go in the default frame
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                (ShowShellBackButton && (NavigationService.Frame.CanGoBack || ForceShowShellBackButton))
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
        }

        public enum AdditionalKinds { Primary, Toast, SecondaryTile, Other, JumpListItem }

        /// <summary>
        /// This determines the simplest case for starting. This should handle 80% of common scenarios. 
        /// When Other is returned the developer must determine start manually using IActivatedEventArgs.Kind
        /// </summary>
        public static AdditionalKinds DetermineStartCause(IActivatedEventArgs args)
        {
            if (args is ToastNotificationActivatedEventArgs)
                return AdditionalKinds.Toast;
            var e = args as ILaunchActivatedEventArgs;
            if (e?.TileId == DefaultTileID && string.IsNullOrEmpty(e?.Arguments))
                return AdditionalKinds.Primary;
            else if (e?.TileId == DefaultTileID && !string.IsNullOrEmpty(e?.Arguments))
                return AdditionalKinds.JumpListItem;
            else if (e?.TileId != null && e?.TileId != DefaultTileID)
                return AdditionalKinds.SecondaryTile;
            else
                return AdditionalKinds.Other;
        }

        private object _PageKeys;
        // T must be a custom Enum
        public Dictionary<T, Type> PageKeys<T>()
            where T : struct, IConvertible
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            if (_PageKeys != null && _PageKeys is Dictionary<T, Type>)
                return _PageKeys as Dictionary<T, Type>;
            return (_PageKeys = new Dictionary<T, Type>()) as Dictionary<T, Type>;
        }
    }
}
