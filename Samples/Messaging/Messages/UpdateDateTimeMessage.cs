using Prism.Events;
using System;

namespace Template10.Samples.MessagingSample.Messages
{
    public class UpdateDateTimeMessage : PubSubEvent<DateTime>
    {
    }
}
