using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.Messaging
{
    public class MessengerService : IMessengerService
    {
        public MessengerService()
        {
            throw new NotImplementedException("Custom implementation required");
        }

        public void Send<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T>(object subscriber, Action<T> callback)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<T>(object subscriber, Action<T> callback)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(object subscriber)
        {
            throw new NotImplementedException();
        }
    }
}
