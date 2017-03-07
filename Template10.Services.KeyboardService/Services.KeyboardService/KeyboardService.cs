using System;
using System.Runtime.CompilerServices;

namespace Template10.Services.KeyboardService
{
    public class KeyboardService : KeyboardServiceBase, IKeyboardService
    {
        public static KeyboardService Instance { get; private set; } = new KeyboardService();

        private KeyboardService()
        {
            Helper.KeyDown = async (e) =>
            {
                e.Handled = true;

                // use this to place focus in search box
                if (e.OnlyControl && e.Character.ToString().ToLower().Equals("e"))
                {
                    AfterControlEGesture?.Invoke();
                }

                // use this to nav back
                else if (e.VirtualKey == Windows.System.VirtualKey.GoBack)
                {
                    AfterBackGesture?.Invoke();
                }

                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationLeft)
                {
                    AfterBackGesture?.Invoke();
                }

                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadMenu)
                {
                    AfterMenuGesture?.Invoke();
                }

                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadLeftShoulder)
                {
                    AfterBackGesture?.Invoke();
                }

                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Back)
                {
                    AfterBackGesture?.Invoke();
                }

                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Left)
                {
                    AfterBackGesture?.Invoke();
                }

                // use this to nav forward
                else if (e.VirtualKey == Windows.System.VirtualKey.GoForward)
                {
                    AfterForwardGesture?.Invoke();
                }
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationRight)
                {
                    AfterForwardGesture?.Invoke();
                }
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadRightShoulder)
                {
                    AfterForwardGesture?.Invoke();
                }
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Right)
                {
                    AfterForwardGesture?.Invoke();
                }

                // about
                else if (e.AltKey && e.ControlKey && e.ShiftKey && e.VirtualKey == Windows.System.VirtualKey.F12)
                {
                    var about = new Windows.UI.Xaml.Controls.ContentDialog
                    {
                        Title = "Template 10",
                        PrimaryButtonText = "Info",
                        SecondaryButtonText = "Close"
                    };

                    try
                    {
                        var result = await about.ShowAsync();
                        if (result == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
                        {
                            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://aka.ms/template10"));
                        }
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        // 
                    }
                }

                // anything else
                else
                    e.Handled = false;
            };
            Helper.PointerGoBackGestured = () =>
            {
                AfterBackGesture?.Invoke();
            };
            Helper.PointerGoForwardGestured = () =>
            {
                AfterForwardGesture?.Invoke();
            };
        }

        public Action AfterBackGesture { get; set; }

        public Action AfterForwardGesture { get; set; }

        public Action AfterControlEGesture { get; set; }

        public Action AfterMenuGesture { get; set; }
    }

}
