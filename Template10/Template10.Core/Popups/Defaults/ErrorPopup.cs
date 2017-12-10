using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class ErrorPopupData : PopupDataBase
    {
        public ErrorPopupData() : base(null, null)
        {
            // invalid
        }

        public ErrorPopupData(Action close, CoreDispatcher dispatcher) : base(close, dispatcher)
        {
            // empty
        }

        private Exception _error;
        public Exception Error
        {
            get => _error;
            set => RaisePropertyChanged(() => _error = value);
        }
    }

    [ContentProperty(Name = nameof(Template))]
    public class ErrorPopup : PopupItemBase<ErrorPopupData>
    {
        public bool Handled { get; set; } = true;

        public override void Initialize()
        {
            Central.Messenger.Subscribe<Messages.UnhandledExceptionMessage>(this, e =>
            {
                Data.Error = e.EventArgs.Exception;
                e.EventArgs.Handled = Handled;
                IsShowing = true;
            });
        }
    }
}