using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;

namespace Template10.Strategies
{
    public partial class DefaultExtendedSessionStrategy : IExtendedSessionStrategy2
    {
        ExtendedSessionManager _manager;

        public DefaultExtendedSessionStrategy()
        {
            _manager = new ExtendedSessionManager();
        }

        bool _StartupAsync = false;
        public async Task StartupAsync(IStartArgsEx e)
        {
            if (Settings.EnableExtendedSessionStrategy)
            {
                if (_StartupAsync)
                {
                    throw new Exception("Startup has alrady been called once.");
                }
                _StartupAsync = true;
                await (this as IExtendedSessionStrategy2).StartUnspecifiedAsync();
            }
        }

        public async Task SuspendingAsync()
        {
            if (Settings.EnableExtendedSessionStrategy)
            {
                await (this as IExtendedSessionStrategy2).StartSaveDataAsync();
            }
        }

        public void Dispose() => _manager.Dispose();
    }

    public partial class DefaultExtendedSessionStrategy
    {
        ExtendedSessionKinds IExtendedSessionStrategy2.CurrentKind => _manager.CurrentKind;

        bool IExtendedSessionStrategy2.IsActive => _manager.IsActive;

        bool IExtendedSessionStrategy2.IsStarted => _manager.IsStarted;

        bool IExtendedSessionStrategy2.IsRevoked => _manager.IsRevoked;

        int IExtendedSessionStrategy2.Progress => _manager.CurrentProgress;

        async Task<bool> IExtendedSessionStrategy2.StartUnspecifiedAsync()
        {
            if (_manager.IsActive)
            {
                return (_manager.CurrentKind == ExtendedSessionKinds.Unspecified);
            }
            else
            {
                return await _manager.StartAsync(ExtendedSessionKinds.Unspecified);
            }
        }

        async Task<bool> IExtendedSessionStrategy2.StartSaveDataAsync()
        {
            if (_manager.IsActive)
            {
                if (_manager.CurrentKind == ExtendedSessionKinds.SavingData)
                {
                    return true;
                }
                else
                {
                    _manager.Create();
                }
            }
            return await _manager.StartAsync(ExtendedSessionKinds.SavingData);
        }
    }
}
