using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;

namespace Blank1.Triggers
{
    public class NetworkAvailableStateTrigger : StateTriggerBase
    {
        public NetworkAvailableStateTrigger()
        {
            NetworkInformation.NetworkStatusChanged -= NetworkInformation_NetworkStatusChanged;
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            UpdateState();
        }

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, UpdateState);
        }

        private void UpdateState()
        {
            bool isConnected = false;
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile != null)
                isConnected = profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            SetActive(
                 isConnected && ConnectionState.Equals(ConnectionStates.Connected) ||
                !isConnected && ConnectionState.Equals(ConnectionStates.Disconnected));
        }

        private ConnectionStates _connectionState;
        public ConnectionStates ConnectionState
        {
            get { return _connectionState; }
            set
            {
                if (_connectionState == value)
                    return;
                _connectionState = value;
                UpdateState();
            }
        }

        public enum ConnectionStates
        {
            Connected,
            Disconnected,
        }
    }
}
