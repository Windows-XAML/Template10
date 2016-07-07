using System;

namespace Template10.Services.NagService
{
    /// <summary>
    /// Indicator of how the user responded to a <see cref="Nag"/>
    /// </summary>
    public enum NagResponse
    {
        /// <summary>
        /// No response has been given (usually means the nag hasn't been shown)
        /// </summary>
        NoResponse,

        /// <summary>
        /// The user performend the <see cref="Nag.NagAction"/>
        /// </summary>
        Accept,

        /// <summary>
        /// The user declined the nag
        /// </summary>
        Decline,

        /// <summary>
        /// The user deferred the nag
        /// </summary>
        Defer
    }

    /// <summary>
    /// Information about a <see cref="Nag"/> registration state and the user's response
    /// </summary>
    public class NagResponseInfo 
    {
        /// <summary>
        /// The unique id of the associated <see cref="Nag"/>
        /// </summary>
        public string NagId { get; set; }

        /// <summary>
        /// Flag indicating to neevr show the nag 
        /// (usually means an error occured deserializing this instance)
        /// </summary>
        public bool Suppress { get; set; }

        /// <summary>
        /// The number of times the app has been launched and the <see cref="Nag"/> registered
        /// </summary>
        public int LaunchCount { get; set; }

        /// <summary>
        /// Timestamp of when the <see cref="Nag"/> was first registered
        /// </summary>
        public DateTimeOffset RegistrationTimeStamp { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// The last response given by the user, if any
        /// </summary>
        public NagResponse LastResponse { get; set; } = NagResponse.NoResponse;

        /// <summary>
        /// Timestamp of when the user was last nagged
        /// </summary>
        public DateTimeOffset LastNag { get; set; } = DateTimeOffset.MaxValue;

        /// <summary>
        /// Flag indicating that the user has not accepted or declined the nag
        /// </summary>
        public bool IsAwaitingResponse
        {
            get { return LastResponse != NagResponse.Accept && LastResponse != NagResponse.Decline; }            
        }

        /// <summary>
        /// Determines if the <see cref="Nag"/> should be shown
        /// </summary>
        /// <param name="launches">The number of app launches to wait for</param>
        /// <returns>True if launches is greater or equal to <see cref="NagResponseInfo.LaunchCount"/>
        /// or launches equals 0</returns>
        public bool ShouldNag(int launches)
        {
            return (!Suppress && LastResponse != NagResponse.Accept && LastResponse != NagResponse.Decline)
                && (launches <= 0 || LaunchCount > launches);
        }

        /// <summary>
        /// Determines if the <see cref="Nag"/> should be shown
        /// </summary>
        /// <param name="duration">The amount of time to wait after the first registartion before showing the <see cref="Nag"/></param>
        /// <returns>True if duration is greater or equal to <see cref="DateTimeOffset.UtcNow"/> + <see cref="NagResponseInfo.RegistrationTimeStamp"/>
        /// or duration equals <see cref="TimeSpan.Zero"/></returns>
        public bool ShouldNag(TimeSpan duration)
        {
            return (!Suppress && LastResponse != NagResponse.Accept && LastResponse != NagResponse.Decline)
                && (duration <= TimeSpan.Zero || DateTimeOffset.UtcNow > RegistrationTimeStamp + duration);
        }
    }
}
