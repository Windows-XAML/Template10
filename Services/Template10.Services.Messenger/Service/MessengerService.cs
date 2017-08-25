using System;

namespace Template10.Services.Messenger
{
    public abstract class MessengerService : IMessengerService
    {
        public MessengerService(IMessengerAdapter adapter)
        {
            _adapter = adapter;
        }
        IMessengerAdapter _adapter { get; set; }
        public void Send<T>(T message)
        {
            LogThis<T>();
            _adapter.Send(message);
        }
        public void Subscribe<T>(object subscriber, Action<T> callback) => _adapter.Subscribe(subscriber, callback);
        public void Unsubscribe<T>(object subscriber, Action<T> callback) => _adapter.Unsubscribe(subscriber, callback);
        public void Unsubscribe(object subscriber) => _adapter.Unsubscribe(subscriber);

        private static void LogThis<T>()
        {
            var message = $"{nameof(MessengerService)}.Send({typeof(T)})";
            try
            {
                var container = Container.ContainerService.Default;
                var logger = container.Resolve<Logging.ILoggingService>();
                logger.WriteLine(message);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine($"LOGGING> {message}");
            }
        }
    }
}
