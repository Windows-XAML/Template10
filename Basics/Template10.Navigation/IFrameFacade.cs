using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Prism.Navigation
{
    public interface IFrameFacade
    {
        bool CanGoBack();
        event EventHandler CanGoBackChanged;
        Task<INavigationResult> GoBackAsync(INavigationParameters parameters, NavigationTransitionInfo infoOverride);

        bool CanGoForward();
        event EventHandler CanGoForwardChanged;
        Task<INavigationResult> GoForwardAsync();

        Task<INavigationResult> RefreshAsync();

        Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameter, NavigationTransitionInfo infoOverride);
    }

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

                return await Orchestrate(
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

                return await Orchestrate(
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

                return await Orchestrate(
                    parameters: null,
                    mode: NavigationMode.Refresh,
                    navigate: navigate);
            }
            catch (Exception ex)
            {
                return NavigationResult.Failure(ex);
            }
        }

        public async Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameter, NavigationTransitionInfo infoOverride)
        {
            try
            {
                if (uri.ToString().StartsWith("/"))
                {
                    _frame.SetNavigationState((new Frame()).GetNavigationState());
                }

                foreach (var path in uri.ToString().Split("/").Where(x => !string.IsNullOrEmpty(x)))
                {
                    var result = await NavigateAsync(
                        path: path,
                        parameter: parameter,
                        infoOverride: infoOverride);

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

        async Task<INavigationResult> NavigateAsync(string path, INavigationParameters parameter, NavigationTransitionInfo infoOverride)
        {
            if (!NavigationService.PageRegistry.TryGetPageInfoType(path, out var info))
            {
                return NavigationResult.Failure($"Path [{path}] failed to get info from {nameof(PageRegistry)}.");
            }

            Func<bool> navigate = () => _frame.Navigate(
                    sourcePageType: info.Page,
                    parameter: new NavigationParameters(path),
                    infoOverride: infoOverride);

            Func<Task<INavigationResult>> orchestrate = async () => await Orchestrate(
                parameters: parameter,
                mode: NavigationMode.New,
                navigate: navigate);

            return await orchestrate();
        }

        private async Task<INavigationResult> Orchestrate(INavigationParameters parameters, NavigationMode mode, Func<bool> navigate)
        {
            parameters = parameters ?? new NavigationParameters();
            (parameters as INavigationParametersInteral).AddInternalParameter(nameof(NavigationMode), mode);
            (parameters as INavigationParametersInteral).AddInternalParameter(nameof(NavigationService), _navigationService);

            // CanNavigateAsync

            if ((_frame.Content as Page)?.DataContext is IConfirmNavigationAsync old_vm_confirma)
            {
                if (!await old_vm_confirma.CanNavigateAsync(parameters))
                {
                    return NavigationResult.Failure($"{old_vm_confirma}.CanNavigateAsync returned false.");
                }
            }

            // CanNavigate

            if ((_frame.Content as Page)?.DataContext is IConfirmNavigation old_vm_confirms)
            {
                if (!old_vm_confirms.CanNavigate(parameters))
                {
                    return NavigationResult.Failure($"{old_vm_confirms}.CanNavigate returned false.");
                }
            }

            // OnNavigatedTo

            if ((_frame.Content as Page)?.DataContext is INavigatedAware old_vm_ed)
            {
                old_vm_ed.OnNavigatedTo(parameters);
            }

            // navigate

            var failed = default(Exception);
            NavigationFailedEventHandler handler = (s, e) =>
            {
                failed = e.Exception;
            };
            _frame.NavigationFailed += handler;
            try
            {
                if (!navigate())
                {
                    throw new Exception("False returned at FrameFacade.Orchestrate().Navigate()");
                }
                else if (failed != null)
                {
                    throw failed;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception raised during FrameFacade.Orchestrate().Navigate()", ex);
            }
            finally
            {
                _frame.NavigationFailed -= handler;
            }

            // OnNavigatingTo

            if ((_frame.Content as Page)?.DataContext is INavigatingAware new_vm_ing)
            {
                new_vm_ing.OnNavigatingTo(parameters);
            }

            // OnNavigatedTo

            if ((_frame.Content as Page)?.DataContext is INavigatedAware new_vm_ed)
            {
                new_vm_ed.OnNavigatedTo(parameters);
            }

            return NavigationResult.Successful();
        }
    }
}
