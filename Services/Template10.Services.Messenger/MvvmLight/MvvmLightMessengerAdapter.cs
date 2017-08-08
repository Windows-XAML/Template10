using System;
using GalaSoft.MvvmLight.Messaging;

namespace Template10.Services.Messenger
{
    public class MvvmLightMessengerService : MessengerService
    {
        public MvvmLightMessengerService()
            : base(new MvvmLightMessengerAdapter())
        {
            // empty
        }
    }

    public class MvvmLightMessengerAdapter : IMessengerAdapter
    {
        private IMessenger _messenger;
        internal MvvmLightMessengerAdapter()
        {
            _messenger = GalaSoft.MvvmLight.Messaging.Messenger.Default;
        }
        public void Send<T>(T message) => _messenger.Send(message);
        public void Subscribe<T>(object subscriber, Action<T> callback) => _messenger.Register(subscriber, callback);
        public void Unsubscribe<T>(object subscriber, Action<T> callback) => _messenger.Unregister(subscriber, callback);
        public void Unsubscribe(object subscriber) => _messenger.Unregister(subscriber);
    }
}
