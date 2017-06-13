using System;
using Template10.Portable.State;
using Template10.Services.SettingsService;

namespace Template10.Services.NavigationService
{

    public class LifecycleLogic : ILifecycleStrategy
    {
        IFrameFacadeInternal FrameFacade { get; }
        public INavigationService NavigationService { get; }

        public LifecycleLogic(IFrameFacadeInternal frame, NavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.FrameFacade = frame;
        }

        public void ClearFrameState()
        {
            Services.NavigationService.NavigationService.DebugWrite();

            GetFrameState().Clear(true);
        }

        public IPersistedStateContainer GetFrameState()
        {
            Services.NavigationService.NavigationService.DebugWrite();

            var container = $"{FrameFacade.FrameId}-Frame-SuspensionState";
            return new SettingsService.SettingsService(SettingsStrategies.Local, container, true);
        }

        public IPersistedStateContainer GetPageState(Type page, int backStackDepth = -1)
        {
            Services.NavigationService.NavigationService.DebugWrite($"page:{page} backStackDepth:{backStackDepth}");

            var folder = $"{page}-{backStackDepth}-Page-SuspensionState";
            return GetPageState(folder);
        }

        public IPersistedStateContainer GetPageState(string folder)
        {
            Services.NavigationService.NavigationService.DebugWrite($"folder:{folder}");

            return GetFrameState().Open(folder, true);
        }

        public void ClearPageState(Type type, int backStackDepth = -1)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            GetPageState(type, backStackDepth).Clear();
        }
    }

}