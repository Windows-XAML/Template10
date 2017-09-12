using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Services.Network;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class NetworkPopupData : Bindable
    {
        private Action _close;
        private CoreDispatcher _dispatcher;

        internal NetworkPopupData(Action close, CoreDispatcher dispatcher)
         : base(dispatcher)
        {
            Close = new Command(close);
        }

        public System.Windows.Input.ICommand Close { get; }

        public NetworkRequirements Requirement { get; set; } = NetworkRequirements.None;

        private ConnectionTypes _actual;
        public ConnectionTypes Actual
        {
            get { return _actual; }
            set
            {
                _actual = value;
                RaisePropertyChanged();
            }
        }
    }

    [ContentProperty(Name = nameof(Template))]
    public class NetworkPopup : PopupItemBase
    {
        public NetworkPopup()
        {
            Content = new NetworkPopupData(() => IsShowing = false, Window.Current.Dispatcher);
        }

        public NetworkRequirements Requirement { get; set; } = NetworkRequirements.None;

        public override void Initialize()
        {
            Central.Network.AvailabilityChanged += Network_AvailabilityChanged;
        }

        public new NetworkPopupData Content
        {
            get => base.Content as NetworkPopupData;
            set => base.Content = value;
        }

        private void Network_AvailabilityChanged(object sender, Services.Network.AvailabilityChangedEventArgs e)
        {
            var showing = false;
            switch (Requirement)
            {
                case NetworkRequirements.None:
                    showing = true;
                    break;
                case NetworkRequirements.NetworkRequired when (e.ConnectionType == Services.Network.ConnectionTypes.LocalNetwork):
                    showing = true;
                    break;
                case NetworkRequirements.InternetRequired when (e.ConnectionType == Services.Network.ConnectionTypes.Internet):
                    showing = true;
                    break;
                default:
                    showing = false;
                    break;
            }
            Content.Requirement = Requirement;
            Content.Actual = e.ConnectionType;
            IsShowing = showing;
        }
    }
}