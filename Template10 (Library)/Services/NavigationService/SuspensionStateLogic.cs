using System;
using Template10.Services.SettingsService;

namespace Template10.Services.NavigationService
{

    public class SuspensionStateLogic
    {
        FrameFacade FrameFacade { get; }
        public INavigationService NavigationService { get; }

        public SuspensionStateLogic(FrameFacade frame, NavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.FrameFacade = frame;
        }

        public ISettingsService GetFrameState()
        {
            Services.NavigationService.NavigationService.DebugWrite();

            var container = $"{FrameFacade.FrameId}-Frame-SuspensionState";
            return SettingsService.SettingsService.Create(SettingsStrategies.Local, container, true);
        }

        public void ClearFrameState()
        {
            Services.NavigationService.NavigationService.DebugWrite();

            GetFrameState().Clear(true);
        }

        public ISettingsService GetPageState(Type page, int backStackDepth = -1)
        {
            Services.NavigationService.NavigationService.DebugWrite($"page:{page} backStackDepth:{backStackDepth}");

            var folder = $"{page}-{backStackDepth}-Page-SuspensionState";
            return GetPageState(folder);
        }

        public ISettingsService GetPageState(string folder)
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