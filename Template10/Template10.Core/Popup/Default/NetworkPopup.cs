using Windows.UI.Xaml.Markup;

namespace Template10.Popup
{
    [ContentProperty(Name = nameof(Template))]
    public class NetworkPopup : PopupItemBase
    {
        public override void Initialize()
        {
            Central.Network.AvailabilityChanged += Network_AvailabilityChanged;
        }

        private void Network_AvailabilityChanged(object sender, Services.Network.AvailabilityChangedEventArgs e)
        {
            var showing = false;
            switch (Settings.NetworkRequirement)
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