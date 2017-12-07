namespace Template10.Navigation
{
    public enum BackButton
    {
        /// <summary>
        /// Attach means when the user clicks the back button either
        /// in the shell or in the app, this navigation service will
        /// GoBack if CanGoBack returns true.
        /// </summary>
        Attach,
        /// <summary>
        /// Ignore means when the user clicks the back button either
        /// in the shell or in the app, this navigation service will
        /// ignore the action and rely on other programatic nagivation.
        /// </summary>
        Ignore
    }
}
