using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.Messaging;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Template10.Services.Gesture
{
    public partial class GestureService : IGestureService
    {
        private IBackButtonService2 _backService2;
        private IKeyboardService2 _keyService2;
        private IMessengerService _messenger;

        public event EventHandler BackButtonUpdated;

        public GestureService(IMessengerService messengerService)
        {
            _keyService2 = new KeyboardService() as IKeyboardService2;
            InitKeyServiceEvents();

            _backService2 = new BackButtonService(_keyService2 as IKeyboardService) as IBackButtonService2;
            _backService2.BackButtonUpdated += (s, e) => BackButtonUpdated?.Invoke(s, e);
            InitBackServiceEvents();

            _messenger = messengerService;
        }

        public void Setup()
        {
            _keyService2.Setup();
            _backService2.Setup();
        }

        public void UpdateBackButton(bool canGoBack, bool canGoForward = false)
        {
            _backService2.UpdateBackButton(canGoBack, canGoForward);
        }

        public EventModes BackRequestedMode { get; set; } = EventModes.Allow;

        public EventModes BackForwardRequestedMode { get; set; } = EventModes.Allow;

        private void InitBackServiceEvents()
        {
            _backService2.BackRequested += (s, e) =>
            {
                _backRequested?.Invoke(s, e);
                if (BackRequestedMode == EventModes.Allow)
                {
                    _messenger.Send(new Messages.BackRequestedMessage());
                }
            };

            _backService2.ForwardRequested += (s, e) =>
            {
                _forwardRequested?.Invoke(s, e);
                if (BackForwardRequestedMode == EventModes.Allow)
                {
                    _messenger.Send(new Messages.ForwardRequestedMessage());
                }
            };
        }

        private void InitKeyServiceEvents()
        {
            _keyService2.AfterSearchGesture += (s, e) =>
            {
                _messenger.Send(new Messages.SearchGestureMessage());
            };

            _keyService2.AfterMenuGesture += (s, e) =>
            {
                _messenger.Send(new Messages.MenuGestureMessage());
            };

            _keyService2.AfterKeyDown += (s, e) =>
            {
                _afterKeyDown?.Invoke(s, e);
            };
        }
    }
}