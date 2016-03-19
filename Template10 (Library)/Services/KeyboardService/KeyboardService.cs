using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Template10.Services.KeyboardService
{
    public class KeyboardService
    {
        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(KeyboardService)}.{caller}");

        #endregion

        KeyboardHelper _helper;

        public static KeyboardService Instance { get; private set; } = new KeyboardService();

        private KeyboardService()
        {
            _helper = new KeyboardHelper();
            _helper.KeyDown = async (e) =>
            {
                e.Handled = true;

                // use this to hide and show the menu
                if (e.WindowsKey && e.Character.ToString().ToLower().Equals("z"))
                {
                    DebugWrite("Windows+Z", caller: nameof(AfterWindowZGesture));
                    AfterWindowZGesture?.Invoke();
                }

                // use this to place focus in search box
                else if (e.OnlyControl && e.Character.ToString().ToLower().Equals("e"))
                {
                    DebugWrite("Control+E", caller: nameof(AfterControlEGesture));
                    AfterControlEGesture?.Invoke();
                }

                // use this to nav back
                else if (e.VirtualKey == Windows.System.VirtualKey.GoBack)
                {
                    DebugWrite("GoBack", caller: nameof(AfterBackGesture));
                    AfterBackGesture?.Invoke();
                }

                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationLeft)
                {
                    DebugWrite("NavigationLeft", caller: nameof(AfterBackGesture));
                    AfterBackGesture?.Invoke();
                }

                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadLeftShoulder)
                {
                    DebugWrite("GamepadLeftShoulder", caller: nameof(AfterBackGesture));
                    AfterBackGesture?.Invoke();
                }

                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Back)
                {
                    DebugWrite("Alt+Back", caller: nameof(AfterBackGesture));
                    AfterBackGesture?.Invoke();
                }

                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Left)
                {
                    DebugWrite("Alt+Left", caller: nameof(AfterBackGesture));
                    AfterBackGesture?.Invoke();
                }

                // use this to nav forward
                else if (e.VirtualKey == Windows.System.VirtualKey.GoForward)
                {
                    DebugWrite("GoForward", caller: nameof(AfterBackGesture));
                    AfterForwardGesture?.Invoke();
                }
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationRight)
                {
                    DebugWrite("NavigationRight", caller: nameof(AfterBackGesture));
                    AfterForwardGesture?.Invoke();
                }
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadRightShoulder)
                {
                    DebugWrite("GamepadRightShoulder", caller: nameof(AfterBackGesture));
                    AfterForwardGesture?.Invoke();
                }
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Right)
                {
                    DebugWrite("Alt+Right", caller: nameof(AfterBackGesture));
                    AfterForwardGesture?.Invoke();
                }

                // about
                else if (e.AltKey && e.ControlKey && e.ShiftKey && e.VirtualKey == Windows.System.VirtualKey.F12)
                {
                    var open = new Action(async () => { await Windows.System.Launcher.LaunchUriAsync(new Uri("http://aka.ms/template10")); });
                    var about = new Windows.UI.Xaml.Controls.ContentDialog
                    {
                        Title = "Template 10",
                        PrimaryButtonText = "Info",
                        PrimaryButtonCommand = new Mvvm.DelegateCommand(open),
                        SecondaryButtonText = "Close"
                    };
                    await about.ShowAsync();
                }

                // anything else
                else
                    e.Handled = false;
            };
            _helper.PointerGoBackGestured = () =>
            {
                DebugWrite(caller: nameof(KeyboardHelper.PointerGoBackGestured));
                AfterBackGesture?.Invoke();
            };
            _helper.PointerGoForwardGestured = () =>
            {
                DebugWrite(caller: nameof(KeyboardHelper.PointerGoForwardGestured));
                AfterForwardGesture?.Invoke();
            };
        }

        public Action AfterBackGesture { get; set; }
        public Action AfterForwardGesture { get; set; }
        public Action AfterControlEGesture { get; set; }
        public Action AfterWindowZGesture { get; set; }
    }

}
