using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;

namespace Template10.Strategies
{
    public partial class DefaultExtendedSessionStrategy : IExtendedSessionStrategy
    {
        ExtendedSessionManager _manager;

        public DefaultExtendedSessionStrategy()
        {
            if (Settings.EnableExtendedSessionStrategy)
            {
                _manager = new ExtendedSessionManager();
            }
        }

        bool _StartupAsync = false;
        public async Task StartupAsync(IStartArgsEx e)
        {
            if (_StartupAsync)
            {
                throw new Exception("Startup has alrady been called once.");
            }
            _StartupAsync = true;
            await (this as IExtendedSessionStrategy2).StartUnspecifiedAsync();
        }

        public async Task SuspendingAsync()
        {
            await (this as IExtendedSessionStrategy2).StartSaveDataAsync();
        }

        public void Dispose() => _manager?.Dispose();
    }

    public partial class DefaultExtendedSessionStrategy : IExtendedSessionStrategy2
    {
        IExtendedSessionStrategy2 Two => this as IExtendedSessionStrategy2;

        ExtendedSessionKinds IExtendedSessionStrategy2.CurrentKind 
            => _manager?.CurrentKind ?? ExtendedSessionKinds.None;

        bool IExtendedSessionStrategy2.IsActive 
            => _manager?.IsActive ?? false;

        bool IExtendedSessionStrategy2.IsStarted 
            => _manager?.IsStarted ?? false;

        bool IExtendedSessionStrategy2.IsRevoked 
            => _manager?.IsRevoked ?? false;

        int IExtendedSessionStrategy2.Progress 
            => _manager?.CurrentProgress ?? default(int);

        async Task<bool> IExtendedSessionStrategy2.StartUnspecifiedAsync()
        {
            if (Two.IsActive)
            {
                return (Two.CurrentKind == ExtendedSessionKinds.Unspecified);
            }
            else
            {
                return await _manager?.StartAsync(ExtendedSessionKinds.Unspecified);
            }
        }

        async Task<bool> IExtendedSessionStrategy2.StartSaveDataAsync()
        {
            if (Two.IsActive)
            {
                if (Two.CurrentKind == ExtendedSessionKinds.SavingData)
                {
                    return true;
                }
                else
                {
                    _manager.Create();
                }
            }
            return await _manager?.StartAsync(ExtendedSessionKinds.SavingData);
        }
    }
}
