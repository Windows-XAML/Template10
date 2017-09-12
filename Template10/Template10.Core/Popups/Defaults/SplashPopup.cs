using System.ComponentModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class SplashPopupData : INotifyPropertyChanged
    {
        private SplashScreen _splashScreen;
        public SplashScreen SplashScreen
        {
            get { return _splashScreen; }
            set
            {
                _splashScreen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SplashScreen)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [ContentProperty(Name = nameof(Template))]
    public class SplashPopup : PopupItemBase
    {
        public bool AutoShow { get; set; } = true;
        public bool AutoHide { get; set; } = true;

        public override void Initialize()
        {
            // empty
        }

        public new SplashPopupData Content
        {
            get => base.Content as SplashPopupData;
            set => base.Content = value;
        }
    }
}