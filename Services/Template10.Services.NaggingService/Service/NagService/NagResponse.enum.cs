namespace Template10.Services.Nag
{
    /// <summary>
    /// Indicator of how the user responded to a <see cref="NagObject"/>
    /// </summary>
    public enum NagResponse
    {
        /// <summary>
        /// No response has been given (usually means the nag hasn't been shown)
        /// </summary>
        NoResponse,

        /// <summary>
        /// The user performend the <see cref="NagObject.NagAction"/>
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
}
