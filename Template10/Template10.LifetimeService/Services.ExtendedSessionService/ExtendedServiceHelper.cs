using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.ExtendedExecution;

namespace Template10.Services.ExtendedSessionService
{
    public class ExtendedServiceHelper : IDisposable
    {
        public enum SessionKinds
        {
            None = -1,
            Unspecified = 0,
            LocationTracking = 1,
            SavingData = 2
        }

        volatile static ExtendedExecutionSession _extendedExecutionSession;

        volatile static SessionKinds _currentKind = SessionKinds.None;
        public SessionKinds CurrentKind => _currentKind;

        public bool IsActive => _isStarted && !_isRevoked;

        volatile static bool _isStarted = false;
        public bool IsStarted => _isStarted;

        volatile static bool _isRevoked = false;
        public bool IsRevoked => _isRevoked;

        public int Progress
        {
            get { return (int)_extendedExecutionSession.PercentProgress; }
            set { if (IsStarted) _extendedExecutionSession.PercentProgress = (uint)value; }
        }

        static ExtendedServiceHelper() => Create();

        static void Create()
        {
            _isStarted = _isRevoked = false;
            _extendedExecutionSession?.Dispose();
            _extendedExecutionSession = new ExtendedExecutionSession
            {
                Description = typeof(ExtendedServiceHelper).ToString(),
            };
            _extendedExecutionSession.Revoked += (s, e) =>
            {
                _isStarted = false;
                _isRevoked = true;
            };
        }

        public async Task<bool> StartAsync(SessionKinds kind)
        {
            try
            {
                switch (kind)
                {
                    case SessionKinds.Unspecified:
                        _extendedExecutionSession.Reason = ExtendedExecutionReason.Unspecified;
                        break;
                    case SessionKinds.LocationTracking:
                        _extendedExecutionSession.Reason = ExtendedExecutionReason.LocationTracking;
                        break;
                    case SessionKinds.SavingData:
                        _extendedExecutionSession.Reason = ExtendedExecutionReason.SavingData;
                        break;
                    default:
                        throw new NotSupportedException(kind.ToString());
                }
                var result = await _extendedExecutionSession.RequestExtensionAsync();
                return result == ExtendedExecutionResult.Allowed;
            }
            catch
            {
                return false;
            }
        }

        public void Stop()
        {
            Create();
        }

        public void Dispose()
        {
            _extendedExecutionSession.Dispose();
        }
    }

}