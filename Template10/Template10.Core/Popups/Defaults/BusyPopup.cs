using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class BusyPopupData : Bindable
    {
        private Action _close;

        internal BusyPopupData(Action close, Windows.UI.Core.CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            Close = new Command(close);
        }

        public System.Windows.Input.ICommand Close { get; }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                RaisePropertyChanged();
            }
        }
    }

    [ContentProperty(Name = nameof(Template))]
    public class BusyPopup : PopupItemBase
    {
        public BusyPopup()
        {
            Content = new BusyPopupData(() => IsShowing = false, Window.Current.Dispatcher);
        }

        public new BusyPopupData Content
        {
            get => base.Content as BusyPopupData;
            set => base.Content = value;
        }

        public override void Initialize()
        {
            // empty
        }
    }
}