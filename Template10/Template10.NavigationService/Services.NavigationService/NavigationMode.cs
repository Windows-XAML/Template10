namespace Template10.Portable.Navigation
{
	public enum NavigationMode
    {
        /// <summary>
        /// Mode is either not set or unknown in this context
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Navigation is to a new instance of a page (not going forward or backward in the stack).
        /// </summary>
        New = 0,
        /// <summary>
        /// Navigation is going backward in the stack.
        /// </summary>
        Back = 1,
        /// <summary>
        /// Navigation is going forward in the stack.
        /// </summary>
        Forward = 2,
        /// <summary>
        /// Navigation is to the current page (perhaps with different data).
        /// </summary>
        Refresh = 3
    }
}