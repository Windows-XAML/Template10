using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.ExtendedExecution;

namespace Template10.Strategies
{
    internal class ExtendedSessionManager : IDisposable
    {
        // static ExtendedSessionManager() => CreateInternal();

        volatile static ExtendedSessionKinds s_currentKind = ExtendedSessionKinds.None;
        public ExtendedSessionKinds CurrentKind => s_currentKind;

        volatile static bool s_isStarted = false;
        public bool IsStarted => s_isStarted;

        volatile static bool s_isRevoked = false;
        public bool IsRevoked => s_isRevoked;

        public bool IsActive => s_isStarted && !s_isRevoked;

        public int CurrentProgress
        {
            get { return (int)s_extendedExecutionSession.PercentProgress; }
            set
            {
                if (!IsStarted)
                {
                    return;
                }
                s_extendedExecutionSession.PercentProgress = (uint)value;
                CurrentProgressChanged?.Invoke(this, value);
            }
        }

        public event TypedEventHandler<int> CurrentProgressChanged;

        volatile static ExtendedExecutionSession s_extendedExecutionSession;

        public async Task<bool> StartAsync(ExtendedSessionKinds kind)
        {
            try
            {
                switch (kind)
                {
                    case ExtendedSessionKinds.Unspecified:
                        s_extendedExecutionSession.Reason = ExtendedExecutionReason.Unspecified;
                        break;
                    case ExtendedSessionKinds.LocationTracking:
                        s_extendedExecutionSession.Reason = ExtendedExecutionReason.LocationTracking;
                        break;
                    case ExtendedSessionKinds.SavingData:
                        s_extendedExecutionSession.Reason = ExtendedExecutionReason.SavingData;
                        break;
                    default:
                        throw new NotSupportedException(kind.ToString());
                }
                var result = await s_extendedExecutionSession.RequestExtensionAsync();
                return s_isStarted = result == ExtendedExecutionResult.Allowed;
            }
            catch
            {
                return false;
            }
        }

        public void Create()
        {
            if (s_extendedExecutionSession != null)
            {
                s_extendedExecutionSession.Revoked -= HandleRevoked;
                s_extendedExecutionSession.Dispose();
            }
            CreateInternal();
        }

        static void CreateInternal()
        {
            s_isStarted = s_isRevoked = false;
            s_extendedExecutionSession = new ExtendedExecutionSession
            {
                Description = typeof(ExtendedSessionManager).ToString(),
            };
            s_extendedExecutionSession.Revoked += HandleRevoked;
        }

        private static void HandleRevoked(object sender, ExtendedExecutionRevokedEventArgs args)
        {
            s_isStarted = false;
            s_isRevoked = true;
        }

        public void Dispose()
        {
            s_extendedExecutionSession?.Dispose();
        }
    }

}