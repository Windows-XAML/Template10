using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;
using Windows.ApplicationModel.ExtendedExecution;

namespace Template10.Strategies
{
    public partial class DefaultExtendedSessionStrategy : IExtendedSessionStrategy
    {
        public bool IsStarted { get; private set; } = false;

        public bool IsRevoked { get; private set; } = false;

        public bool IsActive => IsStarted && !IsRevoked;

        public ExtendedExecutionReason ExReason { get; private set; }

        public async Task StartupAsync(IStartArgsEx e)
        {
            if (Settings.EnableExtendedSessionStrategy)
            {
                await Two.StartUnspecifiedAsync();
            }
        }

        public async Task SuspendingAsync()
        {
            if (Settings.EnableExtendedSessionStrategy)
            {
                await Two.StartSavingDataAsync(null);
            }
        }
    }

    public partial class DefaultExtendedSessionStrategy : IExtendedSessionStrategy2, IDisposable
    {
        IExtendedSessionStrategy2 Two
            => this as IExtendedSessionStrategy2;

        ExtendedExecutionSession IExtendedSessionStrategy2.ExSession { get; set; }

        Action _revokedCallback = null;
        async Task<bool> IExtendedSessionStrategy2.StartSavingDataAsync(Action revokedCallback)
        {
            _revokedCallback = revokedCallback;
            var e = Create(ExtendedExecutionReason.SavingData);
            var result = await e.RequestExtensionAsync();
            return IsStarted = result == ExtendedExecutionResult.Allowed;
        }

        async Task<bool> IExtendedSessionStrategy2.StartUnspecifiedAsync()
        {
            var e = Create(ExtendedExecutionReason.Unspecified);
            var result = await e.RequestExtensionAsync();
            return IsStarted = result == ExtendedExecutionResult.Allowed;
        }

        private ExtendedExecutionSession Create(ExtendedExecutionReason reason)
        {
            DestroyIfExists();
            Two.ExSession = new ExtendedExecutionSession
            {
                Description = GetType().ToString(),
                Reason = reason,
            };
            Two.ExSession.Revoked += _ExtendedExecutionSession_Revoked;
            return Two.ExSession;
        }

        private void DestroyIfExists()
        {
            if (Two.ExSession == null)
            {
                return;
            }
            Two.ExSession.Revoked -= _ExtendedExecutionSession_Revoked;
            Two.ExSession.Dispose();
            IsStarted = IsRevoked = false;
        }

        public event TypedEventHandler<ExtendedExecutionReason> Revoked;
        private void _ExtendedExecutionSession_Revoked(object sender, ExtendedExecutionRevokedEventArgs args)
        {
            IsRevoked = true;
            Revoked?.Invoke(this, ExReason);
            _revokedCallback?.Invoke();
        }

        public void Dispose()
        {
            DestroyIfExists();
        }
    }
}
