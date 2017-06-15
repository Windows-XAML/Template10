using System;
using Template10.Services.ExtendedSessionService;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    //public interface IBootStrapper2
    //{
    //    //static?
    //    ISettings Settings { get; }
    //    IStrategies Strategies { get; }
    //    IStateItems SessionState { get; }

    //    IView[] Views { get; }

    //    Func<SplashScreen, UserControl> SplashFactory { get; set; }

    //    Task OnInitializeAsync();
    //    Task OnStartAsync();
    //    void OnStop();

    //    // OnBackstart
    //}

    //public interface IStrategies
    //{
    //    object LifecycleStrategy { get; }
    //}

    //public class BootstrapperX
    //{
    //    public void OnStart()
    //    {
    //    }

    //    public void OnStop()
    //    {
    //        // if closed
    //        // if suspending
    //    }

    //    public void OnResume(e)
    //    {
    //        RestoreNavigation(TimeSpan.FromDays(2), e);
    //    }

    //    public void OnSuspend(e)
    //    {
    //        SaveNavigation(e);
    //    }
    //}

    //public interface IView
    //{
    //    AppViewBackButtonVisibility ShellBackButtonVisibility { get; set; }
    //    INavigationService[] NavigationServices { get; }
    //}

    //public enum ButtonLocations { Shell, App }

    //public interface ISettings
    //{
    //    ButtonLocations ButtonLocation { get; set; }
    //    TimeSpan CacheMaxDuration { get; set; }
    //    bool AutoRestore { get; set; }
    //}

    public static class Settings
    {
        /// <summary>
        /// The SplashFactory is a Func that returns an instantiated Splash view.
        /// Template 10 will automatically inject this visual before loading the app.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through Settings.SplashStrategy.SplashFactory
        /// </remarks>
        public static Func<SplashScreen, UserControl> SplashFactory
        {
            get => SplashStrategy.SplashFactory;
            set => SplashStrategy.SplashFactory = value;
        }

        /// <summary>
        /// If there is no view-model found for a view then this strategy is used
        /// to attempt to resolve a view-model for a page.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through Services.NavigationService.Settings.ViewModelResolutionStrategy
        /// </remarks>
        public Services.NavigationService.IViewModelResolutionStrategy ViewModelResolutionStrategy
        {
            get => Services.NavigationService.Settings.ViewModelResolutionStrategy;
            set => Services.NavigationService.Settings.ViewModelResolutionStrategy = value;
        }

        public static IWindowStrategy WindowStrategy { get; set; } = new DefaultWindowStrategy();
        public static ISplashStrategy SplashStrategy { get; set; } = new DefaultSplashStrategy();
        public static IExtendedSessionService LifecycleStrategy { get; set; } = new ExtendedSessionService();

        public static bool EnableAutoRestoreAfterTerminated { get; set; } = true;

        /// <summary>
        /// CacheMaxDuration indicates the maximum TimeSpan for which cache data
        /// will be preserved. If Template 10 determines cache data is older than
        /// the specified MaxDuration it will automatically be cleared during start.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through Services.NavigationService.Settings.CacheMaxDuration
        /// </remarks>
        public static TimeSpan CacheMaxDuration
        {
            get => Services.NavigationService.Settings.CacheMaxDuration;
            set => Services.NavigationService.Settings.CacheMaxDuration = value;
        }

        /// <summary>
        /// This property tells Template 10 if should automatically restore the NavitgationState
        /// of Frames when the application is restored from suspension.
        /// </summary>
        public static bool AutoRestoreAfterTerminated { get; set; } = true;

        /// <summary>
        /// This setting tells Template 10 if it should automatically implement a SavingData
        /// ExtendedSession when Suspending. This extends the time limit for Suspension activity.
        /// </summary>
        public static bool AutoExtendExecutionSession { get; set; } = true;

        /// <summary>
        /// This setting tells Template 10 if it should automatically save the NavigationState
        /// of every NavigationService's Frame. This enables it to be restored on Resume.
        /// </summary>
        public static bool AutoSuspendAllFrames { get; set; } = true;

        /// <summary>
        /// When set to true, this will ignore the state of the default Frame.CanGoBack
        /// state and show the button no matter what. Default is false;
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through Services.BackButtonService.Settings.ForceShowShellBackButton
        /// </remarks>
        public static bool ForceShowShellBackButton
        {
            get => Services.BackButtonService.Settings.ForceShowShellBackButton;
            set => Services.BackButtonService.Settings.ForceShowShellBackButton = value;
        }

        /// <summary>
        /// ShowShellBackButton is used to show or hide the shell-drawn back button that
        /// is new to Windows 10. A developer can do this manually, but using this property
        /// is important during navigation because Template 10 manages the visibility
        /// of the shell-drawn back button at that time.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through Services.BackButtonService.Settings.ShowShellBackButton
        /// </remarks>
        public static bool ShowShellBackButton
        {
            get => Services.BackButtonService.Settings.ShowShellBackButton;
            set => Services.BackButtonService.Settings.ShowShellBackButton = value;
        }
    }
}