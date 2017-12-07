using System;
using GalaSoft.MvvmLight.Messaging;
using Template10.Services.Messaging;

namespace Template10
{
    public class DefaultMessengerService : IMessengerService
    {
        private IMessenger _messenger;
        public DefaultMessengerService() => _messenger = new Messenger();
        public void Send<T>(T message) => _messenger.Send<T>(message);
        public void Subscribe<T>(object subscriber, Action<T> callback) => _messenger.Register<T>(subscriber, callback);
        public void Unsubscribe<T>(object subscriber, Action<T> callback) => _messenger.Unregister<T>(subscriber, callback);
        public void Unsubscribe(object subscriber) => _messenger.Unregister(subscriber);
    }
}

