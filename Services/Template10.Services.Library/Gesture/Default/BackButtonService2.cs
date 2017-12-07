using System.ComponentModel;
using Windows.UI.Core;

namespace Template10.Services.Gesture
{
    public partial class BackButtonService : IBackButtonService2
    {
        void IBackButtonService2.Setup()
        {
            _keyboardService.AfterKeyDown += (s, e) =>
            {
                e.Handled = true;

                // use this to nav back
                if (e.VirtualKey == Windows.System.VirtualKey.GoBack) e.Handled = Two.RaiseBackRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationLeft) e.Handled = Two.RaiseBackRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadMenu) e.Handled = Two.RaiseBackRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadLeftShoulder) e.Handled = Two.RaiseBackRequested().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Back) e.Handled = Two.RaiseBackRequested().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Left) e.Handled = Two.RaiseBackRequested().Handled;

                // use this to nav forward
                else if (e.VirtualKey == Windows.System.VirtualKey.GoForward) e.Handled = Two.RaiseForwardRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationRight) e.Handled = Two.RaiseForwardRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadRightShoulder) e.Handled = Two.RaiseForwardRequested().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Right) e.Handled = Two.RaiseForwardRequested().Handled;
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e)
                => e.Handled = Two.RaiseBackRequested().Handled;
        }

        public IBackButtonService2 Two => this as IBackButtonService2;

        HandledEventArgs IBackButtonService2.RaiseBackRequested()
        {
            var cancelEventArgs = new CancelEventArgs();
            BeforeBackRequested?.Invoke(null, cancelEventArgs);
            if (cancelEventArgs.Cancel)
            {
                return new HandledEventArgs { Handled = true };
            }

            var handledEventArgs = new HandledEventArgs();
            BackRequested?.Invoke(null, handledEventArgs);
            return handledEventArgs;
        }

        HandledEventArgs IBackButtonService2.RaiseForwardRequested()
        {
            var cancelEventArgs = new CancelEventArgs();
            BeforeForwardRequested?.Invoke(null, cancelEventArgs);
            if (cancelEventArgs.Cancel)
            {
                return new HandledEventArgs { Handled = true };
            }

            var handledEventArgs = new HandledEventArgs();
            ForwardRequested?.Invoke(null, handledEventArgs);
            return handledEventArgs;
        }
    }
}
