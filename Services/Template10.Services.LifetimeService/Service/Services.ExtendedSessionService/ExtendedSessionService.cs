using System;
using System.Threading.Tasks;

namespace Template10.Services.ExtendedSessionService
{
    public class ExtendedSessionService : IExtendedSessionService, IDisposable
    {
        public ExtendedServiceHelper _helper { get; }

        public ExtendedSessionService()
        {
            _helper = new ExtendedServiceHelper();
        }

        public async Task<bool> StartUnspecifiedAsync()
        {
            if (_helper.IsActive)
            {
                if (_helper.CurrentKind == ExtendedServiceHelper.SessionKinds.Unspecified)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return await _helper.StartAsync(ExtendedServiceHelper.SessionKinds.Unspecified);
            }
        }

        public async Task<bool> StartSaveDataAsync()
        {
            if (_helper.IsActive)
            {
                if (_helper.CurrentKind == ExtendedServiceHelper.SessionKinds.SavingData)
                {
                    return true;
                }
                _helper.Stop();
            }
            return await _helper.StartAsync(ExtendedServiceHelper.SessionKinds.SavingData);
        }

        public void Dispose()
        {
            _helper.Dispose();
        }
    }
}
