using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Template10.Mvvm.Messenger
{
    // use messenger to communicate between viewmodels
    public class Messenger : GalaSoft.MvvmLight.Messaging.Messenger
    {
        public void Subscribe<T>(object recipient, Action<T> action)where T : IMessage
        {
            base.Register<T>(recipient, action);
        }

        public void Unsubscribe<T>(object recipient)where T : IMessage
        {
            base.Unregister<T>(recipient);
        }

        public void Publish<T>(T message)where T : IMessage
        {
            base.Send<T>(message);
        }
    }
}