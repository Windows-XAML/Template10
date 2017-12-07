using System;
using System.Threading.Tasks;
using Template10.Services.Messaging;

namespace Template10.Services.Network
{
    public class NetworkService : INetworkService
    {
        static NetworkAvailableHelper _helper;
        static IMessengerService _messengerService;

        static NetworkService()
        {
            _helper = new NetworkAvailableHelper();
            _helper.AvailabilityChanged += (e) =>
            {
                _messengerService?.Send(new Messages.NetworkAvailabilityChangedMessage(e));
            };
        }

        public NetworkService(IMessengerService messengerService)
        {
            _messengerService = messengerService;
        }

        public async Task<bool> GetIsInternetAvailableAsync()
            => await _helper.GetIsInternetAvailableAsync();

        public async Task<bool> GetIsNetworkAvailableAsync()
            => await _helper.GetIsNetworkAvailableAsync();
    }
}
