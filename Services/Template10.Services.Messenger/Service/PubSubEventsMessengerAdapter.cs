using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Template10.Services.Messenger
{

    public class PubSubEventsMessengerAdapter : IMessengerAdapter
    {
        private IEventAggregator _default;
        public PubSubEventsMessengerAdapter() => _default = new EventAggregator();

        public void Send<T>(T message) => _default.GetEvent<PubSubEvent<T>>().Publish(message);
        public void Subscribe<T>(object subscriber, Action<T> callback) => _default.GetEvent<PubSubEvent<T>>().Subscribe(callback);
        public void Unsubscribe<T>(object subscriber, Action<T> callback) => _default.GetEvent<PubSubEvent<T>>().Unsubscribe(callback);
        public void Unsubscribe(object subscriber) => throw new NotSupportedException("Prism does not support this technique.");
    }
}
