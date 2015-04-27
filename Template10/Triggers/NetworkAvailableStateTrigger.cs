using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;

namespace Template10.Triggers
{
	public class NetworkAvailableStateTrigger : StateTriggerBase
    {
        public NetworkAvailableStateTrigger()
        {
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
            // TODO: complete check
            if (profile != null)
                isConnected = profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            SetActive(
                 isConnected && ConnectionState == ConnectionState.Connected ||
                !isConnected && ConnectionState == ConnectionState.Disconnected);
        }

        public ConnectionState ConnectionState
        {
            get { return (ConnectionState)GetValue(ConnectionStateProperty); }
            set { SetValue(ConnectionStateProperty, value); }
        }

        public static readonly DependencyProperty ConnectionStateProperty =
            DependencyProperty.Register("ConnectionState", typeof(ConnectionState), typeof(NetworkAvailableStateTrigger),
            new PropertyMetadata(ConnectionState.Connected, OnConnectionStatePropertyChanged));

        private static void OnConnectionStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (NetworkAvailableStateTrigger)d;
            obj.UpdateState();
        }
    }

    public enum ConnectionState
    {
        Connected,
        Disconnected,
    }

}
