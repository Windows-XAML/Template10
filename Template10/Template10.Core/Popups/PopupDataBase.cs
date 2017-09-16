using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace Template10.Popups
{
    public class PopupDataBase : IPopupData
    {
        CoreDispatcher _dispatcher;

        internal PopupDataBase(Action close, CoreDispatcher dispatcher)
        {
            Close = new Command(close);
            _dispatcher = dispatcher;
        }

        public System.Windows.Input.ICommand Close { get; }

        private string _text;
        public string Text
        {
            get => _text;
            set => RaisePropertyChanged(() => _text = value);
        }

        private bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set => RaisePropertyChanged(() => _isActive = value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async void RaisePropertyChanged(Action action, [CallerMemberName] string property = null)
        {
            action();
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property)));
        }
    }
}