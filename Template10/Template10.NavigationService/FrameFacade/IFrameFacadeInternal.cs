using System;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.NavigationService
{
    public interface IFrameFacadeInternal : IFrameFacade
    {
        Frame Frame { get; set; }
        INavigationService NavigationService { get; set; }

        string GetNavigationState();
        void SetNavigationState(string state);
        void PurgeNavigationState();

        void GoBack();
        void GoBack(NavigationTransitionInfo infoOverride);
        void GoForward();

        bool CanGoBack { get; }
        bool CanGoForward { get; }

        bool Navigate(Type page, object parameter, NavigationTransitionInfo info);

        Task<FrameState> GetFrameStateAsync();
        Task<IPersistedDictionary> GetPageStateAsync(Type page);

        void ClearCache(bool removeCachedPagesInBackStack = false);
    }
}