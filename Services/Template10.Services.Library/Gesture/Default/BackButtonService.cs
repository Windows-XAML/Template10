using System;
using System.ComponentModel;
using Template10.Services.Gesture;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Template10.Services.Gesture
{

    public partial class BackButtonService : IBackButtonService
    {
        IKeyboardService2 _keyboardService;
        internal BackButtonService(IKeyboardService keyboardService)
        {
            _keyboardService = keyboardService as IKeyboardService2;
        }

        /// <summary>
        /// This event allows a mechanism to intercept BackRequested and stop it. Some
        /// use cases would include the ModalDialog which would cancel the event, using
        /// it instead for itself to close the dialog - not wanting it to navigate a frame.
        /// </summary>
        public event TypedEventHandler<object, CancelEventArgs> BeforeBackRequested;
        public event TypedEventHandler<object, HandledEventArgs> BackRequested;

        /// <summary>
        /// This event allows a mechanism to intercept ForwardRequested and stop it. 
        /// </summary>
        public event TypedEventHandler<object, CancelEventArgs> BeforeForwardRequested;
        public event TypedEventHandler<object, HandledEventArgs> ForwardRequested;

        public event EventHandler BackButtonUpdated;

        public void UpdateBackButton(bool canGoBack, bool canGoForward = false)
        {
            switch (Settings.ShellBackButtonPreference)
            {
                case ShellBackButtonPreferences.AlwaysShowInShell when (!Settings.ShellBackButtonVisible):
                    Settings.ShellBackButtonVisible = true;
                    BackButtonUpdated?.Invoke(null, EventArgs.Empty);
                    break;
                case ShellBackButtonPreferences.AutoShowInShell when (!Settings.ShellBackButtonVisible):
                    Settings.ShellBackButtonVisible = canGoBack;
                    BackButtonUpdated?.Invoke(null, EventArgs.Empty);
                    break;
                case ShellBackButtonPreferences.NeverShowInShell when (Settings.ShellBackButtonVisible):
                    Settings.ShellBackButtonVisible = false;
                    BackButtonUpdated?.Invoke(null, EventArgs.Empty);
                    break;
                default:
                    Settings.ShellBackButtonVisible = false;
                    BackButtonUpdated?.Invoke(null, EventArgs.Empty);
                    break;
            }
        }
    }
}
