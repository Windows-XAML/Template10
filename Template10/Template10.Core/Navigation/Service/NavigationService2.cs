using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Common;
using Template10.Extensions;
using Template10.Strategies;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public partial class NavigationService : INavigationService2
    {
        INavigationService2 Two => this as INavigationService2;

        void INavigationService2.RaiseNavigated(NavigatedEventArgs e)
        {
            Navigated?.Invoke(this, e);
        }

        void INavigationService2.RaiseNavigatingCancels(object parameter, bool suspending, NavigationMode mode, NavigationInfo toInfo, out ContinueResult cancel)
        {
            var navigatingDeferral = new Common.DeferralExManager();
            var navigatingEventArgs = new NavigatingEventArgs(navigatingDeferral)
            {
                Parameter = parameter,
                Suspending = suspending,
                NavigationMode = mode,
                TargetPageType = toInfo.PageType,
                TargetPageParameter = toInfo.Parameter,
            };
            Navigating?.Invoke(this, navigatingEventArgs);
            cancel = navigatingEventArgs.Cancel ? ContinueResult.Stop : ContinueResult.Continue;
        }

        void INavigationService2.RaiseBeforeSavingNavigation(out bool cancel)
        {
            var args = new CancelEventArgsEx<Type>(CurrentPageType);
            BeforeSavingNavigation?.Invoke(this, args);
            cancel = args.Cancel;
        }
        void INavigationService2.RaiseAfterRestoreSavedNavigation()
            => _afterRestoreSavedNavigation?.Invoke(this, CurrentPageType);

        void INavigationService2.RaiseBackRequested(HandledEventArgsEx args)
            => BackRequested?.Invoke(this, args);

        void INavigationService2.RaiseForwardRequested(HandledEventArgsEx args)
            => ForwardRequested?.Invoke(this, args);

        TypedEventHandler<Type> _afterRestoreSavedNavigation;
        event TypedEventHandler<Type> INavigationService2.AfterRestoreSavedNavigation
        {
            add => _afterRestoreSavedNavigation += value;
            remove => _afterRestoreSavedNavigation -= value;
        }

        async Task INavigationService2.SaveAsync(bool navigateFrom)
        {
            // save navigation state into settings

            this.Log($"Frame: {FrameEx.FrameId}");

            if (CurrentPageType == null) return;
            Two.RaiseBeforeSavingNavigation(out var cancel);
            if (cancel)
            {
                return;
            }

            if (navigateFrom)
            {
                var vm = (FrameEx.Content as Page)?.DataContext;
                var fromInfo = new NavigationInfo(CurrentPageType, CurrentPageParam, await FrameEx2.GetPageStateAsync(CurrentPageType));
                await ViewModelActionStrategy.NavigatingFromAsync(vm, NavigationMode.New, true, fromInfo, null, this);
                await ViewModelActionStrategy.NavigatedFromAsync(vm, NavigationMode.New, true, fromInfo, null, this);
            }

            var frameState = await FrameEx2.GetFrameStateAsync();
            var navigationState = FrameEx2.GetNavigationState();
            await frameState.SetNavigationStateAsync(navigationState);

#if DEBUG
            var writtenState = await frameState.TryGetNavigationStateAsync();
            this.Log($"navigationState:{navigationState} writtenState:{writtenState}");
            Debug.Assert(navigationState.Equals(writtenState.Value), "Checking frame nav state save");
#endif

            await Task.CompletedTask;
        }

        async Task<bool> INavigationService2.LoadAsync(bool navigateTo, NavigationMode mode)
        {
            this.Log($"Frame: {FrameEx.FrameId}");

            try
            {
                // load navigation state from settings
                var frameState = await FrameEx2.GetFrameStateAsync();
                {
                    var state = await frameState.TryGetNavigationStateAsync();
                    this.Log($"After TryGetNavigationStateAsync; state: {state.Value ?? "Null"}");
                    if (state.Success && !string.IsNullOrEmpty(state.Value?.ToString()))
                    {
                        FrameEx2.SetNavigationState(state.Value.ToString());
                        await frameState.SetNavigationStateAsync(string.Empty);
                    }
                    else
                    {
                        return false;
                    }
                }

                // is there a page there?
                var newPage = FrameEx.Content as Page;
                if (newPage == null)
                {
                    return false;
                }
                else
                {
                    CurrentPageType = newPage.GetType();
                }


                // continue?

                if (!navigateTo)
                {
                    return true;
                }

                // resolve?

                var viewModel = await ViewModelResolutionStrategy.ResolveViewModelAsync(newPage);
                if (viewModel == null)
                {
                    viewModel = newPage?.DataContext;
                }

                // known type?

                if (viewModel is ITemplate10ViewModel vm && vm != null)
                {
                    vm.NavigationService = this;
                }

                // setup?

                if (viewModel != null)
                {
                    var toState = await FrameEx2.GetPageStateAsync(CurrentPageType);
                    var currentPageParam = DeserializeParameter(FrameEx.BackStack.Last().Parameter);
                    var toInfo = new NavigationInfo(CurrentPageType, currentPageParam, toState);
                    await ViewModelActionStrategy.NavigatingToAsync(viewModel, mode, true, null, toInfo, this);
                    await ViewModelActionStrategy.NavigatedToAsync(viewModel, mode, true, null, toInfo, this);
                }

                return true;
            }
            catch { return false; }
            finally { Two.RaiseAfterRestoreSavedNavigation(); }
        }

        private object DeserializeParameter(object parameter)
        {
            if (CurrentPageParam is string s && Central.Serialization.TryDeserialize<object>(s, out var deserialized_parameter))
            {
                return deserialized_parameter;
            }
            else
            {
                return parameter;
            }
        }

        async Task INavigationService2.SuspendAsync()
        {
            this.Log($"Frame: {FrameEx.FrameId}");

            var dispatcher = this.Window.Dispatcher;
            await dispatcher.DispatchAsync(async () =>
            {
                var vm = (FrameEx.Content as Page)?.DataContext;
                var fromInfo = new NavigationInfo(CurrentPageType, CurrentPageParam, await FrameEx2.GetPageStateAsync(CurrentPageType));
                await ViewModelActionStrategy.NavigatedFromAsync(vm, NavigationMode.New, true, fromInfo, null, this);
            });
        }
    }
}

