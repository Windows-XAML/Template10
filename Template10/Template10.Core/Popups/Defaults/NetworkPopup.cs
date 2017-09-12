using System.ComponentModel;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class NetworkPopupData : INotifyPropertyChanged
    {
        public NetworkRequirements Requirement { get; set; } = NetworkRequirements.None;

        private NetworkRequirements _actual;
        public NetworkRequirements Actual
        {
            get { return _actual; }
            set
            {
                _actual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Actual)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [ContentProperty(Name = nameof(Template))]
    public class NetworkPopup : PopupItemBase
    {
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
            IsShowing = showing;
        }
    }
}