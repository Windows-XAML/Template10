using System;

namespace Template10.Services.MarketplaceService
{
    class NagInfo
    {
        public bool Suppress { get; set; }

        public int LaunchCount { get; set; }

        public DateTimeOffset FirstRegistered { get; set; }
    }
}
