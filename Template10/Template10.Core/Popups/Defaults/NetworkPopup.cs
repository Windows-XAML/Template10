using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Services.Network;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class NetworkPopupData : PopupDataBase
    {
        internal NetworkPopupData(Action close, CoreDispatcher dispatcher) : base(close, dispatcher)
        {
            // empty 
        }

        private NetworkRequirements _requirement;
        public NetworkRequirements Requirement
        {
            get => _requirement;
            set => RaisePropertyChanged(() => _requirement = value);
        }

        private ConnectionTypes _actual;
        public ConnectionTypes Actual
        {
            get => _actual; 
            set=> RaisePropertyChanged(() => _actual = value);
        }
    }

    [ContentProperty(Name = nameof(Template))]
    public class NetworkPopup : PopupItemBase<NetworkPopupData>
    {
        public NetworkRequirements Requirement { get; set; } = NetworkRequirements.None;

        public override void Initialize()
        {
            Central.Network.AvailabilityChanged += (s, e) =>
            {
                var showing = false;
                switch (Requirement)
                {
                    case NetworkRequirements.None:
                        showing = true;
                        break;
                    case NetworkRequirements.Network when (e.ConnectionType == Services.Network.ConnectionTypes.LocalNetwork):
                        showing = true;
                        break;
                    case NetworkRequirements.Internet when (e.ConnectionType == Services.Network.ConnectionTypes.Internet):
                        showing = true;
                        break;
                    default:
                        showing = false;
                        break;
                }
                Content.Requirement = Requirement;
                Content.Actual = e.ConnectionType;
                IsShowing = showing;
            };
        }
    }
}