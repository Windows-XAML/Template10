using System;
using System.ComponentModel;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class ErrorPopupData : INotifyPropertyChanged
    {
        private Exception _error;
        public Exception Error
        {
            get { return _error; }
            set
            {
                _error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [ContentProperty(Name = nameof(Template))]
    public class ErrorPopup : PopupItemBase
    {
        public override void Initialize()
        {
            // empty
        }

        public new ErrorPopupData Content
        {
            get => base.Content as ErrorPopupData;
            set => base.Content = value;
        }
    }
}