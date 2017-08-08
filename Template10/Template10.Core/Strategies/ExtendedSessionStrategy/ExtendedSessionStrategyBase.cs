using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;

namespace Template10.Strategies
{
    public abstract class ExtendedSessionStrategyBase : IExtendedSessionStrategy2
    {
        ExtendedSessionManager _manager;
        public ExtendedSessionStrategyBase()
        {
            _manager = new ExtendedSessionManager();
        }

        public abstract Task StartupAsync(IStartArgsEx e);

        public abstract Task SuspendingAsync();

        public virtual void Dispose()
        {
            _manager.Dispose();
        }

        ExtendedSessionKinds IExtendedSessionStrategy2.CurrentKind => _manager.CurrentKind;

        bool IExtendedSessionStrategy2.IsActive => _manager.IsActive;

        bool IExtendedSessionStrategy2.IsStarted => _manager.IsStarted;

        bool IExtendedSessionStrategy2.IsRevoked => _manager.IsRevoked;

        int IExtendedSessionStrategy2.Progress => _manager.CurrentProgress;

        async Task<bool> IExtendedSessionStrategy2.StartUnspecifiedAsync()
        {
            if (_manager.IsActive)
            {
                if (_manager.CurrentKind == ExtendedSessionKinds.Unspecified)
                {
                    return true;
                }
                return false;
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
                _manager.Create();
            }
            return await _manager.StartAsync(ExtendedSessionKinds.SavingData);
        }
    }
}
