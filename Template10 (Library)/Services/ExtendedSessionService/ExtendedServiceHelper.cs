using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.ExtendedExecution;

namespace Template10.Services.ExtendedSessionService
{
    internal class ExtendedServiceHelper
    {
        static ExtendedExecutionSession _session;

        public ExtendedServiceHelper()
        {
            CreateUnspecified();
        }

        private void CreateUnspecified()
        {
            if (_session != null)
            {
                return;
            }
            _session = new ExtendedExecutionSession
            {
                Reason = ExtendedExecutionReason.Unspecified
            };
            _session.Revoked += (s, e) => WasRevoked = true;
            AllowedToStart = null;
            WasRevoked = null;
        }

        public async Task<bool> StartUnspecifiedAsync()
        {
            var result = await _session.RequestExtensionAsync();
            if (result == ExtendedExecutionResult.Allowed)
            {
                AllowedToStart = true;
                WasRevoked = false;
            }
            else
            {
                AllowedToStart = false;
                WasRevoked = true;
            }
            return AllowedToStart.Value;
        }

        public void StopUnspecified()
        {
            _session.Dispose();
            CreateUnspecified();
        }

        public bool? AllowedToStart { get; set; }
        public bool? WasRevoked { get; set; }
    }

}