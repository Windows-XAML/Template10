using System;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Navigation
{
    public interface ITemplate10FrameInternal : ITemplate10Frame
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

        Task<Template10FrameState> GetFrameStateAsync();
        Task<IPersistedDictionary> GetPageStateAsync(Type page);
        Task<IPersistedDictionary> GetPageStateAsync(string page);

        void ClearCache(bool removeCachedPagesInBackStack = false);
    }
}