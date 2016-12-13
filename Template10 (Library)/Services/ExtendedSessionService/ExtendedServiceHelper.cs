using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.ExtendedExecution;

namespace Template10.Services.ExtendedSessionService
{
    public class ExtendedServiceHelper: IDisposable
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
        volatile static bool _isStarted = false;
        volatile static bool _isRevoked = false;

        public SessionKinds CurrentKind => _currentKind;
        public bool IsActive => _isStarted && !_isRevoked;
        public bool IsStarted => _isStarted;
        public bool IsRevoked => _isRevoked;

        public int Progress
        {
            get { return (int)_extendedExecutionSession.PercentProgress; }
            set { if (IsStarted) _extendedExecutionSession.PercentProgress = (uint)value; }
        }

        static ExtendedServiceHelper()
        {
            Create();
        }

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
                    case SessionKinds.None:
                        throw new NotSupportedException();
                    case SessionKinds.Unspecified:
                        _extendedExecutionSession.Reason = ExtendedExecutionReason.Unspecified;
                        break;
                    case SessionKinds.LocationTracking:
                        _extendedExecutionSession.Reason = ExtendedExecutionReason.LocationTracking;
                        break;
                    case SessionKinds.SavingData:
                        _extendedExecutionSession.Reason = ExtendedExecutionReason.SavingData;
                        break;
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