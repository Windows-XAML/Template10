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
        event EventHandler Navigated;

        string Id { get; }

        Task SuspendAsync();

        Task ResumeAsync();

        Task<bool> NavigateAsync<P>(Type page, P parameter = default(P), NavigationTransitionInfo infoOverride = null) 
            where P: struct, IConvertible;

        Task<bool> NavigateAsync<T, P>(T key, P parameter = default(P), NavigationTransitionInfo infoOverride = null) 
            where T : struct, IConvertible 
            where P : struct, IConvertible;

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

