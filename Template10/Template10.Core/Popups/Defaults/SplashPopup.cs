using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class SplashPopupData : PopupDataBase
    {
        internal SplashPopupData(Action close, CoreDispatcher dispatcher) : base(close, dispatcher)
        {
            // empty
        }

        private SplashScreen _splashScreen;
        public SplashScreen SplashScreen
        {
            get => _splashScreen;
            set => RaisePropertyChanged(() => _splashScreen = value);
        }
    }

    [ContentProperty(Name = nameof(Template))]
    public class SplashPopup : PopupItemBase<SplashPopupData>
    {
        public bool AutoShow { get; set; } = true;
        public bool AutoHide { get; set; } = true;

        public override void Initialize()
        {
            Content = new SplashPopupData(() => IsShowing = false, Window.Current.Dispatcher);
        }
    }
}