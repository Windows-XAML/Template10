using Prism.Navigation;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public class FrameFacade : IFrameFacade
    {
        private Frame _frame;
        private readonly CoreDispatcher _dispatcher;
        public event EventHandler CanGoBackChanged;
        public event EventHandler CanGoForwardChanged;
        private readonly INavigationServiceUwp _navigationService;

        public FrameFacade(Frame frame, INavigationServiceUwp navigationService)
        {
            _frame = frame;
            _dispatcher = frame.Dispatcher;
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

        public async Task<INavigationResult> GoBackAsync(INavigationParameters parameters,
            NavigationTransitionInfo infoOverride)
        {
            if (!CanGoBack())
            {
                return NavigationResult.Failure($"{nameof(CanGoBack)} is false.");
            }

            return await NavigateAsync(
              parameters: parameters,
              mode: Prism.Navigation.NavigationMode.Back,
              navigate: async () =>
              {
                  _frame.GoBack(infoOverride);
                  return true;
              });
        }

        public async Task<INavigationResult> GoForwardAsync()
        {
            if (!CanGoForward())
            {
                return NavigationResult.Failure($"{nameof(CanGoForward)} is false.");
            }

            return await NavigateAsync(
                parameters: null,
                mode: Prism.Navigation.NavigationMode.Forward,
                navigate: async () =>
                {
                    _frame.GoForward();
                    return true;
                });
        }

        public async Task<INavigationResult> RefreshAsync()
        {
            return await NavigateAsync(
                parameters: null,
                mode: Prism.Navigation.NavigationMode.Refresh,
                navigate: async () =>
                {
                    _frame.SetNavigationState(_frame.GetNavigationState());
                    return !_frame.BackStack.Any();
                });
        }

        async Task<INavigationResult> NavigateAsync(
            string path,
            INavigationParameters parameter,
            NavigationTransitionInfo infoOverride)
        {
            return await NavigateAsync(
                queue: NavigationQueue.Parse(path, parameter),
                infoOverride: infoOverride);
        }

        public async Task<INavigationResult> NavigateAsync(
            Uri path,
            INavigationParameters parameter,
            NavigationTransitionInfo infoOverride)
        {
            return await NavigateAsync(
                queue: NavigationQueue.Parse(path, parameter),
                infoOverride: infoOverride);
        }

        private async Task<INavigationResult> NavigateAsync(
            NavigationQueue queue,
            NavigationTransitionInfo infoOverride)
        {
            Debug.WriteLine($"{nameof(FrameFacade)}.{nameof(NavigateAsync)}({queue})");

            if (queue.ClearBackStack)
            {
                // create Clear()
                _frame.SetNavigationState(new Frame().GetNavigationState());
            }

            while (queue.Count > 0)
            {
                var pageNavinfo = queue.Dequeue();

                var result = await NavigateAsync(
                    pageNavInfo: pageNavinfo,
                    infoOverride: infoOverride);

                if (!result.Success)
                {
                    return result;
                }
            }

            return NavigationResult.Successful();
        }

        private async Task<INavigationResult> NavigateAsync(
            PageNavigationInfo pageNavInfo,
            NavigationTransitionInfo infoOverride)
        {
            Debug.WriteLine($"{nameof(FrameFacade)}.{nameof(NavigateAsync)}({pageNavInfo})");

            return await NavigateAsync(
                parameters: pageNavInfo.Parameters,
                mode: Prism.Navigation.NavigationMode.New,
                navigate: async () =>
                {
                    if (_dispatcher.HasThreadAccess)
                    {
                        return _frame.Navigate(
                          sourcePageType: pageNavInfo.PageType,
                          parameter: pageNavInfo.Parameters,
                          infoOverride: infoOverride);
                    }
                    else
                    {
                        var result = false;
                        await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            result = _frame.Navigate(
                              sourcePageType: pageNavInfo.PageType,
                              parameter: pageNavInfo.Parameters,
                              infoOverride: infoOverride);
                        });
                        return result;
                    }
                });
        }

        private async Task<INavigationResult> NavigateAsync(
            INavigationParameters parameters,
            Prism.Navigation.NavigationMode mode,
            Func<Task<bool>> navigate)
        {
            // default parameters

            parameters = CreateDefaultParameters(parameters, mode);

            // hold prev vm

            var old_vm = (_frame.Content as Page)?.DataContext;
            if (old_vm == null)
            {
                Debug.WriteLine($"[From]View-Model is null.");
            }
            else
            {
                // CanNavigateAsync

                if (old_vm is IConfirmNavigationAsync old_vm_confirma)
                {
                    Debug.WriteLine($"[From]{old_vm_confirma}.{nameof(IConfirmNavigationAsync)} calling.");
                    var confirm = await old_vm_confirma.CanNavigateAsync(parameters);
                    Debug.WriteLine($"[From]{old_vm_confirma}.{nameof(IConfirmNavigationAsync)} is {confirm}.");
                    if (!confirm)
                    {
                        return NavigationResult.Failure($"[From]{old_vm_confirma}.CanNavigateAsync returned false.");
                    }
                }
                else
                {
                    Debug.WriteLine($"[From]{nameof(IConfirmNavigationAsync)} not implemented.");
                }

                // CanNavigate

                if (old_vm is IConfirmNavigation old_vm_confirms)
                {
                    Debug.WriteLine($"[From]{old_vm_confirms}.{nameof(IConfirmNavigation)} calling.");
                    var confirm = old_vm_confirms.CanNavigate(parameters);
                    Debug.WriteLine($"[From]{old_vm_confirms}.{nameof(IConfirmNavigation)} is {confirm}.");
                    if (!confirm)
                    {
                        return NavigationResult.Failure($"[From]{old_vm_confirms}.CanNavigate returned false.");
                    }
                }
                else
                {
                    Debug.WriteLine($"[From]{nameof(IConfirmNavigation)} not implemented.");
                }
            }

            // navigate

            var nav_result = await NavigateFrameAsync(navigate);
            Debug.WriteLine($"NavigateFrameAsync returned {nav_result}.");
            if (!nav_result)
            {
                return NavigationResult.Failure("Navigate() failed.");
            }

            // OnNavigatedFrom

            if (old_vm != null)
            {
                if (old_vm is INavigatedAware old_vm_ed)
                {
                    Debug.WriteLine($"[From]{nameof(INavigatedAware)}.OnNavigatedFrom() calling.");
                    old_vm_ed.OnNavigatedFrom(parameters);
                }
                else
                {
                    Debug.WriteLine($"[From]{nameof(INavigatedAware)} not implemented.");
                }
            }

            // hold new vm

            var new_vm = (_frame.Content as Page)?.DataContext;

            if (new_vm == null)
            {
                Debug.WriteLine($"[To]View-Model is null.");
            }
            else
            {
                // OnNavigatingTo

                if (new_vm is INavigatingAware new_vm_ing)
                {
                    Debug.WriteLine($"[To]{nameof(INavigatingAware)}.OnNavigatingTo() calling.");
                    new_vm_ing.OnNavigatingTo(parameters);
                }
                else
                {
                    Debug.WriteLine($"[To]{nameof(INavigatingAware)} not implemented.");
                }

                // OnNavigatedTo

                if (new_vm is INavigatedAware new_vm_ed)
                {
                    Debug.WriteLine($"[To]{nameof(INavigatedAware)}.OnNavigatedTo() calling.");
                    new_vm_ed.OnNavigatedTo(parameters);
                }
                else
                {
                    Debug.WriteLine($"[To]{nameof(INavigatedAware)} not implemented.");
                }
            }

            // finally

            return NavigationResult.Successful();
        }

        private INavigationParameters CreateDefaultParameters(INavigationParameters parameters, Prism.Navigation.NavigationMode mode)
        {
            parameters = parameters ?? new NavigationParameters();
            parameters.SetNavigationMode(mode);
            parameters.SetNavigationService(_navigationService);
            return parameters;
        }

        private async Task<bool> NavigateFrameAsync(Func<Task<bool>> navigate)
        {
            void failedHandler(object s, NavigationFailedEventArgs e)
            {
                throw e.Exception;
            }
            try
            {
                _frame.NavigationFailed += failedHandler;
                return await Task.Run(navigate);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in FrameFacade.NavigateFrameAsync().", ex);
            }
            finally
            {
                _frame.NavigationFailed -= failedHandler;
            }
        }
    }
}
