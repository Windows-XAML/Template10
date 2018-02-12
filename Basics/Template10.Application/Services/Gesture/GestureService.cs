using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;

namespace Prism.Windows.Services
{
    public class GestureService : IGestureService
    {
        public event EventHandler MenuRequested;
        public event EventHandler BackRequested;
        public event EventHandler SearchRequested;
        public event EventHandler RefreshRequested;
        public event EventHandler ForwardRequested;
        public event TypedEventHandler<object, KeyDownEventArgs> KeyDown;

        public GestureService()
        {
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) => BackRequested?.Invoke(null, EventArgs.Empty);
        }

        public void RaiseBackRequested()
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }

        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
        {
            var properties = e.CurrentPoint.Properties;

            // Ignore button chords with the left, right, and middle buttons
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
                properties.IsMiddleButtonPressed)
                return;

            TestForNavigateRequested(e, properties);
        }

        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
        {
            if (!e.EventType.ToString().Contains("Down") || e.Handled)
            {
                return;
            }
            var args = new KeyDownEventArgs(e.VirtualKey)
            {
                EventArgs = e
            };

            TestForSearchRequested(args);
            TestForMenuRequested(args);
            TestForNavigateRequested(args);
            KeyDown?.Invoke(null, args);
        }

        private void TestForNavigateRequested(KeyDownEventArgs e)
        {
            if ((e.VirtualKey == VirtualKey.GoBack)
             || (e.VirtualKey == VirtualKey.NavigationLeft)
             || (e.VirtualKey == VirtualKey.GamepadMenu)
             || (e.VirtualKey == VirtualKey.GamepadLeftShoulder)
             || (e.OnlyAlt && e.VirtualKey == VirtualKey.Back)
             || (e.OnlyAlt && e.VirtualKey == VirtualKey.Left))
            {
                Debug.WriteLine($"{nameof(GestureService)}.{nameof(BackRequested)}");
                BackRequested?.Invoke(null, EventArgs.Empty);
            }
            else if ((e.VirtualKey == VirtualKey.GoForward)
             || (e.VirtualKey == VirtualKey.NavigationRight)
             || (e.VirtualKey == VirtualKey.GamepadRightShoulder)
             || (e.OnlyAlt && e.VirtualKey == VirtualKey.Right))
            {
                Debug.WriteLine($"{nameof(GestureService)}.{nameof(ForwardRequested)}");
                ForwardRequested?.Invoke(null, EventArgs.Empty);
            }
            else if ((e.VirtualKey == VirtualKey.Refresh)
             || (e.VirtualKey == VirtualKey.F5))
            {
                Debug.WriteLine($"{nameof(GestureService)}.{nameof(RefreshRequested)}");
                RefreshRequested?.Invoke(null, EventArgs.Empty);
            }
        }

        private void TestForNavigateRequested(PointerEventArgs e, PointerPointProperties properties)
        {
            // If back or foward are pressed (but not both) 
            var backPressed = properties.IsXButton1Pressed;
            var forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                e.Handled = true;
                if (backPressed)
                {
                    Debug.WriteLine($"{nameof(GestureService)}.{nameof(BackRequested)}");
                    BackRequested?.Invoke(null, EventArgs.Empty);
                }
                else if (forwardPressed)
                {
                    Debug.WriteLine($"{nameof(GestureService)}.{nameof(ForwardRequested)}");
                    ForwardRequested?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        private void TestForMenuRequested(KeyDownEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.GamepadMenu)
            {
                Debug.WriteLine($"{nameof(GestureService)}.{nameof(MenuRequested)}");
                MenuRequested?.Invoke(null, EventArgs.Empty);
            }
        }

        private void TestForSearchRequested(KeyDownEventArgs args)
        {
            if (args.OnlyControl && args.Character.ToString().ToLower().Equals("e"))
            {
                Debug.WriteLine($"{nameof(GestureService)}.{nameof(SearchRequested)}");
                SearchRequested?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
