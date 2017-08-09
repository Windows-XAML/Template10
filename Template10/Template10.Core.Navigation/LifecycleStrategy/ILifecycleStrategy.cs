using System;
using Template10.Portable.State;

namespace Template10.Services.NavigationService
{
    public interface ILifecycleStrategy
    {
        INavigationService NavigationService { get; }

        void ClearFrameState();

        void ClearPageState(Type type, int backStackDepth = -1);

        IPersistedStateContainer GetFrameState();

        IPersistedStateContainer GetPageState(string folder);

        IPersistedStateContainer GetPageState(Type page, int backStackDepth = -1);
    }
}