using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Common;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public interface IFrameEx2
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

        Task<IFrameExState> GetFrameStateAsync();
        Task<IPropertyBagEx> GetPageStateAsync(Type page);
        Task<IPropertyBagEx> GetPageStateAsync(string page);

        void ClearNavigationCache(bool removeCachedPagesInBackStack = false);

        Strategies.INavStateStrategy StateStrategy { get; }
    }
}