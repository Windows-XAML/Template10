using System.ComponentModel;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class BusyPopupData : INotifyPropertyChanged
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [ContentProperty(Name = nameof(Template))]
    public class BusyPopup : PopupItemBase
    {
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