using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace Template10.Popups
{
    public abstract class Bindable : INotifyPropertyChanged
    {
        private CoreDispatcher _dispatcher;

        public Bindable(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async void RaisePropertyChanged([CallerMemberName] string property = null)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property)));
        }
    }
}