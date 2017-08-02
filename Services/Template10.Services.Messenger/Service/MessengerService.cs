using System;

namespace Template10.Services.Messenger
{
    public class MessengerService : IMessengerService
    {
        private static IMessengerService _instance;
        public static IMessengerService Instance => GetInstance();
        public static IMessengerService GetInstance() => _instance ?? (_instance = new MessengerService(Settings.DefaultAdapter));
        private MessengerService(IMessengerAdapter adapter) => _adapter = adapter;

        public IMessengerAdapter _adapter { get; set; }

        public void Send<T>(T message) => _adapter.Send(message);
        public void Subscribe<T>(object subscriber, Action<T> callback) => _adapter.Subscribe(subscriber, callback);
        public void Unsubscribe<T>(object subscriber, Action<T> callback) => _adapter.Unsubscribe(subscriber, callback);
        public void Unsubscribe(object subscriber) => _adapter.Unsubscribe(subscriber);
    }
}
