using System;

namespace Template10.Services.NagService
{
    public enum NagResponse
    {
        NoResponse,
        Accept,
        Decline,
        Defer
    }

    public class NagResponseInfo 
    {
        public string NagId { get; set; }

        public bool Suppress { get; set; }

        public int LaunchCount { get; set; }

        public DateTimeOffset RegistrationTimeStamp { get; set; } = DateTimeOffset.UtcNow;

        public NagResponse LastResponse { get; set; } = NagResponse.NoResponse;

        public DateTimeOffset LastNag { get; set; } = DateTimeOffset.MaxValue;

        public bool ShouldNag(int launches)
        {
            return (!Suppress && LastResponse != NagResponse.Accept && LastResponse != NagResponse.Decline)
                && (launches <= 0 || LaunchCount > launches);
        }

        public bool ShouldNag(TimeSpan duration)
        {
            return (!Suppress && LastResponse != NagResponse.Accept && LastResponse != NagResponse.Decline)
                && (duration <= TimeSpan.Zero || DateTimeOffset.UtcNow > RegistrationTimeStamp + duration);
        }

        public NagResponseInfo Increment()
        {
            var clone = (NagResponseInfo)MemberwiseClone();
            clone.LaunchCount++;
            return clone;
        }
    }
}
