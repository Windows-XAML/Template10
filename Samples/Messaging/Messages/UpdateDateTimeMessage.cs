using Prism.Events;
using System;

namespace Messaging.Messages
{
    public class UpdateDateTimeMessage : PubSubEvent<DateTime>
    {
    }
}
