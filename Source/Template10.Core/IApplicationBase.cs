using System;
using System.Threading.Tasks;
using Prism.Ioc;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Template10
{
    public interface ISuspend
    {
    }

    public interface IApplicationBase
    {
        /*
         
         STARTUP EXECUTION ORDER
         * 01. PrismApplicationBase.InternalInitialize
         * 02. PrismApplicationBase.RegisterRequiredTypes
         * 03. PrismApplicationBase.InternalStartAsync(Args:Windows.ApplicationModel.Activation.LaunchActivatedEventArgs Kind:Launch Cause:PrimaryTile)
         * 04. PrismApplicationBase.CallOnInitializedOnce
          
         THEN
         * 05. NavigationService.NavigateAsync(MainPage)
         * 06. FrameFacade.NavigateAsync(MainPage)
         * 07. FrameFacade.NavigateAsync(MainPage)
         * 08. [From]View-Model is null.
         * 09. FrameFacade.NavigateFrameAsync HasThreadAccess: True
         * 10. FrameFacade.OrchestrateAsync.NavigateFrameAsync() returned True.
         * 11. Calling OnNavigatedToAsync
         * 12. INavigatedAwareAsync.OnNavigatedToAsync() called.
         * 13. Calling OnNavigatingTo
         * 14. INavigatingAware not implemented.
         * 15. Calling OnNavigatedTo
         * 16. INavigatedAware not implemented.
         
         */
        Func<SplashScreen, UIElement> ExtendedSplashScreenFactory { get; set; }
        IContainerProvider Container { get; }
        void ConfigureViewModelLocator();
        IContainerExtension CreateContainerExtension();
        void OnInitialized();
        void OnStart(IStartArgs args);
        Task OnStartAsync(IStartArgs args);
        void RegisterTypes(IContainerRegistry container);
    }
}
