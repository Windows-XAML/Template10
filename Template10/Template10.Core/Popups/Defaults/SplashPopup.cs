using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class SplashPopupData : Bindable
    {
        private Action _close;
        private CoreDispatcher _dispatcher;

        internal SplashPopupData(Action close, CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            Close = new Command(close);
        }

        public System.Windows.Input.ICommand Close { get; }

        private SplashScreen _splashScreen;
        public SplashScreen SplashScreen
        {
            get { return _splashScreen; }
            set
            {
                _splashScreen = value;
                RaisePropertyChanged();
            }
        }
    }

    [ContentProperty(Name = nameof(Template))]
    public class SplashPopup : PopupItemBase
    {
        public bool AutoShow { get; set; } = true;
        public bool AutoHide { get; set; } = true;

        public override void Initialize()
        {
            Content = new SplashPopupData(() => IsShowing = false, Window.Current.Dispatcher);
        }

        public new SplashPopupData Content
        {
            get => base.Content as SplashPopupData;
            set => base.Content = value;
        }
    }
}