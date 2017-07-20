using System;
using GalaSoft.MvvmLight.Messaging;

namespace Template10.Services.Messenger
{
    public class MvvmLightMessengerAdapter : IMessengerAdapter
    {
        private IMessenger _default;
        public MvvmLightMessengerAdapter() => _default = GalaSoft.MvvmLight.Messaging.Messenger.Default;

        public void Send<T>(T message) => _default.Send(message);
        public void Subscribe<T>(object subscriber, Action<T> callback) => _default.Register(subscriber, callback);
        public void Unsubscribe<T>(object subscriber, Action<T> callback) => _default.Unregister(subscriber, callback);
        public void Unsubscribe(object subscriber) => _default.Unregister(subscriber);
    }
}
