using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;

namespace Template10.Strategies
{
    public abstract class ExtendedSessionStrategyBase : IExtendedSessionStrategyInternal
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

        ExtendedSessionKinds IExtendedSessionStrategyInternal.CurrentKind => _manager.CurrentKind;

        bool IExtendedSessionStrategyInternal.IsActive => _manager.IsActive;

        bool IExtendedSessionStrategyInternal.IsStarted => _manager.IsStarted;

        bool IExtendedSessionStrategyInternal.IsRevoked => _manager.IsRevoked;

        int IExtendedSessionStrategyInternal.Progress => _manager.CurrentProgress;

        async Task<bool> IExtendedSessionStrategyInternal.StartUnspecifiedAsync()
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

        async Task<bool> IExtendedSessionStrategyInternal.StartSaveDataAsync()
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
