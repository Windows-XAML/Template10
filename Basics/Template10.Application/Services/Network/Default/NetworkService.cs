using System;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Windows.Messages;

namespace Prism.Windows.Services.Network
{
    public class NetworkService : INetworkService
    {
        private IEventAggregator _eventAggregator;
        private NetworkAvailableHelper _helper;

        public NetworkService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _helper = new NetworkAvailableHelper();
            _helper.AvailabilityChanged += (e) =>
            {
                _eventAggregator.GetEvent<PubSubEvent<NetworkAvailabilityChangedMessage>>()
                    .Publish(new NetworkAvailabilityChangedMessage(e));
            };
        }

        public async Task<bool> GetIsInternetAvailableAsync()
            => await _helper.GetIsInternetAvailableAsync();

        public async Task<bool> GetIsNetworkAvailableAsync()
            => await _helper.GetIsNetworkAvailableAsync();
    }
}
