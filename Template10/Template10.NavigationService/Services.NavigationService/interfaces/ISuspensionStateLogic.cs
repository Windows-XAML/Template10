using System;
using Template10.Services.SettingsService;

namespace Template10.Services.NavigationService
{
	public interface ISuspensionStateLogic
    {
        INavigationService NavigationService { get; }
        void ClearFrameState();
        void ClearPageState(Type type, int backStackDepth = -1);
        ISettingsService GetFrameState();
        ISettingsService GetPageState(string folder);
        ISettingsService GetPageState(Type page, int backStackDepth = -1);
    }
}