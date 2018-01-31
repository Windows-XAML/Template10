using Prism.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public class FrameFacade : IFrameFacade
    {
        private Frame _frame;
        private readonly INavigationServiceUwp _navigationService;
        public event EventHandler CanGoBackChanged;
        public event EventHandler CanGoForwardChanged;

        public FrameFacade(Frame frame, INavigationServiceUwp navigationService)
        {
            _frame = frame;
            _navigationService = navigationService;
            _frame.RegisterPropertyChangedCallback(Frame.CanGoBackProperty, (s, p)
                => CanGoBackChanged?.Invoke(this, EventArgs.Empty));
            _frame.RegisterPropertyChangedCallback(Frame.CanGoForwardProperty, (s, p)
                => CanGoForwardChanged?.Invoke(this, EventArgs.Empty));
        }

        public bool CanGoBack()
            => _frame.CanGoBack;

        public bool CanGoForward()
            => _frame.CanGoForward;

        public async Task<INavigationResult> GoBackAsync(INavigationParameters parameters, NavigationTransitionInfo infoOverride)
        {
            try
            {
                if (!CanGoBack())
                {
                    return NavigationResult.Failure($"{nameof(CanGoBack)} is false.");
                }

                Func<bool> navigate = () =>
                {
                    _frame.GoBack(infoOverride);
                    return true;
                };

                return await OrchestrateNavigation(
                  parameters: parameters,
                  mode: NavigationMode.Back,
                  navigate: navigate);
            }
            catch (Exception ex)
            {
                return NavigationResult.Failure(ex);
            }
        }

        public async Task<INavigationResult> GoForwardAsync()
        {
            try
            {
                if (!CanGoForward())
                {
                    return NavigationResult.Failure($"{nameof(CanGoForward)} is false.");
                }

                Func<bool> navigate = () =>
                {
                    _frame.GoForward();
                    return true;
                };

                return await OrchestrateNavigation(
                    parameters: null,
                    mode: NavigationMode.Forward,
                    navigate: navigate);
            }
            catch (Exception ex)
            {
                return NavigationResult.Failure(ex);
            }
        }

        public async Task<INavigationResult> RefreshAsync()
        {
            try
            {
                Func<bool> navigate = () =>
                {
                    _frame.SetNavigationState(_frame.GetNavigationState());
                    return !_frame.BackStack.Any();
                };

                return await OrchestrateNavigation(
                    parameters: null,
                    mode: NavigationMode.Refresh,
                    navigate: navigate);
            }
            catch (Exception ex)
            {
                return NavigationResult.Failure(ex);
            }
        }

        async Task<INavigationResult> NavigateAsync(string path, INavigationParameters parameter, NavigationTransitionInfo infoOverride)
           => await NavigateAsync(new Uri(path, UriKind.Relative), parameter, infoOverride);

        public async Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameter, NavigationTransitionInfo infoOverride)
        {
            try
            {
                var queue = PageNavigationRegistry.ParsePath(uri, parameter);

                if (queue.ClearBackStack)
                {
                    _frame.SetNavigationState(new Frame().GetNavigationState());
                }

                while (queue.Count > 0)
                {
                    var pageNavigationInfo = queue.Dequeue();

                    Func<bool> navigate = () => _frame.Navigate(
                       sourcePageType: pageNavigationInfo.Page,
                       parameter: pageNavigationInfo.Parameters,
                       infoOverride: infoOverride);

                    Func<Task<INavigationResult>> orchestrate = async () => await OrchestrateNavigation(
                        parameters: parameter,
                        mode: NavigationMode.New,
                        navigate: navigate);

                    var result = await orchestrate();

                    if (!result.Success)
                    {
                        return result;
                    }
                }

                return NavigationResult.Successful();
            }
            catch (Exception ex)
            {
                return NavigationResult.Failure(ex);
            }
        }

        private async Task<INavigationResult> OrchestrateNavigation(INavigationParameters parameters, Prism.Navigation.NavigationMode mode, Func<bool> navigate)
        {
            // default parameters

            parameters = parameters ?? new NavigationParameters();
            parameters.SetNavigationMode(mode);
            parameters.SetNavigationService(_navigationService);

            // hold prev vm

            var old_vm = (_frame.Content as Page)?.DataContext;

            // CanNavigateAsync

            if (old_vm is IConfirmNavigationAsync old_vm_confirma)
            {
                if (!await old_vm_confirma.CanNavigateAsync(parameters))
                {
                    return NavigationResult.Failure($"{old_vm_confirma}.CanNavigateAsync returned false.");
                }
            }

            // CanNavigate

            if (old_vm is IConfirmNavigation old_vm_confirms)
            {
                if (!old_vm_confirms.CanNavigate(parameters))
                {
                    return NavigationResult.Failure($"{old_vm_confirms}.CanNavigate returned false.");
                }
            }

            // navigate

            await NavigateInternalAsync(navigate);

            // OnNavigatedFrom

            if (old_vm is INavigatedAware old_vm_ed)
            {
                old_vm_ed.OnNavigatedFrom(parameters);
            }

            // hold new vm

            var new_vm = (_frame.Content as Page)?.DataContext;

            // OnNavigatingTo

            if (new_vm is INavigatingAware new_vm_ing)
            {
                new_vm_ing.OnNavigatingTo(parameters);
            }

            // OnNavigatedTo

            if (new_vm is INavigatedAware new_vm_ed)
            {
                new_vm_ed.OnNavigatedTo(parameters);
            }

            // finally

            return NavigationResult.Successful();
        }

        private async Task NavigateInternalAsync(Func<bool> navigate)
        {
            void failed(object s, NavigationFailedEventArgs e) => throw e.Exception;
            try
            {
                _frame.NavigationFailed += failed;
                await Task.Run(navigate);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in FrameFacade.NavigateInternalAsync()/navigate().", ex);
            }
            finally
            {
                _frame.NavigationFailed -= failed;
            }
        }
    }
}
