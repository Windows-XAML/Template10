using System;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.Navigation
{
    public interface INavigationService
    {
        event EventHandler Suspending;
        event EventHandler Suspended;
        event EventHandler Resuming;
        event EventHandler Resumed;
        event EventHandler<Type> Navigating;
        event EventHandler<bool> Navigated;

        string Id { get; }

        Task SuspendAsync();

        Task ResumeAsync();

        Task<bool> NavigateAsync(Type page, string parameter = null, NavigationTransitionInfo infoOverride = null);

        Task<bool> NavigateAsync(Type page, IPropertySet parameter = null, NavigationTransitionInfo infoOverride = null);

        Task<bool> NavigateAsync<T>(T key, string parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;

        Task<bool> NavigateAsync<T>(T key, IPropertySet parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;

        bool CanGoBack { get; }

        Task<bool> GoBackAsync(NavigationTransitionInfo infoOverride = null);

        bool CanGoForward { get; }

        Task<bool> GoForwardAsync();

        Page CurrentPage { get; }

        string CurrentParameter { get; }

        object CurrentViewModel { get; }

        NavigationModes CurrentNavigationMode { get; }
    }
}

