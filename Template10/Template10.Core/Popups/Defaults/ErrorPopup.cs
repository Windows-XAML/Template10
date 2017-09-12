using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class ErrorPopupData : Bindable
    {
        private Action _close;
        private CoreDispatcher _dispatcher;

        internal ErrorPopupData(Action close, CoreDispatcher dispatcher)
          : base(dispatcher)
        {
            Close = new Command(close);
        }

        public System.Windows.Input.ICommand Close { get; }

        private Exception _error;
        public Exception Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged();
            }
        }
    }

    [ContentProperty(Name = nameof(Template))]
    public class ErrorPopup : PopupItemBase
    {
        public ErrorPopup()
        {
            Content = new ErrorPopupData(() => IsShowing = false, Window.Current.Dispatcher);
        }

        public override void Initialize()
        {
            Central.Messenger.Subscribe<Messages.UnhandledExceptionMessage>(this, e =>
            {
                Content.Error = e.EventArgs.Exception;
                IsShowing = true;
            });
        }

        public new ErrorPopupData Content
        {
            get => base.Content as ErrorPopupData;
            set => base.Content = value;
        }
    }
}