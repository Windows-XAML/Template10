using System;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Template10.Strategies;
using Template10.Utils;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    public class Settings
    {
        public static IExtendedSessionStrategy ExtendedSessionStrategy { get; set; } // = new DefaultExtendedSessionStrategy();

        public static ILifecycleStrategy SuspendResumeStrategy { get; set; } = new DefaultLifecycleStrategy();

        public static ITitleBarStrategy TitleBarStrategy { get; set; } = new DefaultTitleBarStrategy();

        public static IPersistedDictionaryFactory PersistenceStrategyFactory
        {
            get => Services.NavigationService.Settings.PersistedDictionaryFactory;
            set => Services.NavigationService.Settings.PersistedDictionaryFactory = value;
        }

        public static Func<SplashScreen, UserControl> SplashFactory { get; set; }

        /// <summary>
        ///  By default, Template 10 will setup the root element to be a Template 10
        ///  Modal Dialog control. If you desire something different, you can set it here.
        /// </summary>
        public static Func<Template10StartArgs, Task<UIElement>> RootFactoryAsync { get; set; } = new Func<Template10StartArgs, Task<UIElement>>(async (e) =>
        {
            // return new ModalDialog { Content = await new Frame().RegisterAsync() };
            return await new Frame().CreateNavigationService();
        });

        /// <summary>
        /// If there is no view-model found for a view then this strategy is used
        /// to attempt to resolve a view-model for a page.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through Services.NavigationService.Settings.ViewModelResolutionStrategy
        /// </remarks>
        public static Services.NavigationService.IViewModelResolutionStrategy ViewModelResolutionStrategy
        {
            get => Services.NavigationService.Settings.ViewModelResolutionStrategy;
            set => Services.NavigationService.Settings.ViewModelResolutionStrategy = value;
        }

        /// <summary>
        /// Template 10 automatically stores state, but that state can go stale. 
        /// Use CacheMaxDuration to indicate the desired max age of any state. 
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
        /// This setting tells Template 10 if it should automatically implement a SavingData
        /// ExtendedSession when Suspending. This extends the time limit for Suspension activity.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through Services.NavigationService.Settings.CacheMaxDuration
        /// </remarks>
        public static bool AutoExtendExecutionSession
        {
            get => Strategies.Settings.AutoExtendExecutionSession;
            set => Strategies.Settings.AutoExtendExecutionSession = value;
        }


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
        /// is new to Windows 10. A developer may have a back button within the canvas.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through Services.BackButtonService.Settings.ShowShellBackButton
        /// </remarks>
        public static bool ShowShellBackButton
        {
            get => Services.BackButtonService.Settings.ShellBackButtonVisible;
            set => Services.BackButtonService.Settings.ShellBackButtonVisible = value;
        }
    }
}