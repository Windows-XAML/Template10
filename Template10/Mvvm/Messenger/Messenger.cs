using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Template10.Mvvm.Messenger
{
    // use messenger to communicate between viewmodels
    public class Messenger : GalaSoft.MvvmLight.Messaging.Messenger
    {
        public void Subscribe<Message>(object recipient, Action<Message> action)
        {
            base.Register<Message>(recipient, action);
        }

        public void Unsubscribe<Message>(object recipient)
        {
            base.Unregister<Message>(recipient);
        }

        public void Publish<Message>(Message message)
        {
            base.Send<Message>(message);
        }
    }
}