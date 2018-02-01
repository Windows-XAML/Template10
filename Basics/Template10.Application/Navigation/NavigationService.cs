using Prism.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Navigation
{
    public class NavigationService : IPlatformNavigationService
    {
        private readonly IFrameFacade _frame;

        public NavigationService(Frame frame)
        {
            _frame = new FrameFacade(frame, this);
            _frame.CanGoBackChanged += (s, e) =>
                CanGoBackChanged?.Invoke(this, EventArgs.Empty);
            _frame.CanGoForwardChanged += (s, e) =>
                CanGoForwardChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task RefreshAsync()
            => await _frame.RefreshAsync();

        // go forward

        public event EventHandler CanGoForwardChanged;

        public bool CanGoForward()
            => _frame.CanGoForward();

        public async Task<INavigationResult> GoForwardAsync()
            => await _frame.GoForwardAsync();

        // go back

        public event EventHandler CanGoBackChanged;

        public bool CanGoBack()
            => _frame.CanGoBack();

        public async Task<INavigationResult> GoBackAsync()
            => await GoBackAsync(
                parameters: default(INavigationParameters),
                infoOverride: default(NavigationTransitionInfo));

        public async Task<INavigationResult> GoBackAsync(INavigationParameters parameters)
            => await GoBackAsync(
                parameters: default(INavigationParameters),
                infoOverride: default(NavigationTransitionInfo));

        public async Task<INavigationResult> GoBackAsync(INavigationParameters parameters = null, NavigationTransitionInfo infoOverride = null)
            => await _frame.GoBackAsync(
                parameters: parameters,
                infoOverride: infoOverride);

        // navigate(string)

        public async Task<INavigationResult> NavigateAsync(string path)
            => await NavigateAsync(
                uri: new Uri(path, UriKind.RelativeOrAbsolute),
                parameter: default(INavigationParameters),
                infoOverride: default(NavigationTransitionInfo));

        public async Task<INavigationResult> NavigateAsync(string path, INavigationParameters parameters)
            => await NavigateAsync(
                uri: new Uri(path, UriKind.RelativeOrAbsolute),
                parameter: parameters,
                infoOverride: default(NavigationTransitionInfo));

        public async Task<INavigationResult> NavigateAsync(string path, INavigationParameters parameter, NavigationTransitionInfo infoOverride)
            => await NavigateAsync(
                uri: new Uri(path, UriKind.RelativeOrAbsolute),
                parameter: parameter,
                infoOverride: infoOverride);

        // navigate(uri)

        public async Task<INavigationResult> NavigateAsync(Uri uri)
            => await NavigateAsync(
                uri: uri,
                parameter: default(INavigationParameters),
                infoOverride: default(NavigationTransitionInfo));

        public async Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameters)
            => await NavigateAsync(
                uri: uri,
                parameter: parameters,
                infoOverride: default(NavigationTransitionInfo));

        public async Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameter, NavigationTransitionInfo infoOverride)
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(NavigationService)}.{nameof(NavigateAsync)}({uri})");
            return await _frame.NavigateAsync(
                uri: uri,
                parameter: parameter,
                infoOverride: infoOverride);
        }
    }
}
