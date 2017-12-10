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
        public SplashPopupData() : base(null, null)
        {
            // invalid
        }

        public SplashPopupData(Action close, CoreDispatcher dispatcher) : base(close, dispatcher)
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
        public override void Initialize()
        {
            Data = new SplashPopupData(() => IsShowing = false, Window.Current.Dispatcher);
        }
    }
}