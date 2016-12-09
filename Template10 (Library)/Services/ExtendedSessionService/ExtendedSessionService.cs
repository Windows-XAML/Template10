using System.Threading.Tasks;

namespace Template10.Services.ExtendedSessionService
{
    public class ExtendedSessionService : IExtendedSessionService
    {
        ExtendedServiceHelper _helper;

        public ExtendedSessionService()
        {
            _helper = new ExtendedServiceHelper();
        }

        internal async Task StartAsync()
        {
            await _helper.StartUnspecifiedAsync();
        }

        public enum ClosingStatuses
        {
            IsClosing, IsNotClosing, Unknown
        }

        public ClosingStatuses ApplicationClosingStatus
        {
            get
            {
                if (!_helper.AllowedToStart ?? false)
                {
                    return ClosingStatuses.Unknown;
                }
                else if (_helper.WasRevoked ?? false)
                {
                    return ClosingStatuses.IsClosing;
                }
                else
                {
                    return ClosingStatuses.IsNotClosing;
                }
            }
        }
    }
}
