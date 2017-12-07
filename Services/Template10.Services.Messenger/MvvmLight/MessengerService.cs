using System;
using GalaSoft.MvvmLight.Messaging;
using Template10.Services.Messaging;

namespace Template10.Impl
{
    public class MessengerService : IMessengerService
    {
        private IMessenger _messenger;
        internal MessengerService()
        {
            _messenger = GalaSoft.MvvmLight.Messaging.Messenger.Default;
        }

        public void Send<T>(T message) 
            => _messenger.Send(message);

        public void Subscribe<T>(object subscriber, Action<T> callback)
            => _messenger.Register(subscriber, callback);

        public void Unsubscribe<T>(object subscriber, Action<T> callback) 
            => _messenger.Unregister(subscriber, callback);

        public void Unsubscribe(object subscriber) 
            => _messenger.Unregister(subscriber);
    }
}
