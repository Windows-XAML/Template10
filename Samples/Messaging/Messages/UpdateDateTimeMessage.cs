using System;
using Prism.Events;

namespace Template10.Samples.MessagingSample.Messages
{
    public class UpdateDateTimeMessage : PubSubEvent<DateTime>
    {
    }
}
