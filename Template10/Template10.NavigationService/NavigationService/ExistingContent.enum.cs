using System;

namespace Template10.Services.NavigationService
{
    [Obsolete]
    public enum ExistingContent
    {
        /// <summary>
        /// Include means that when the current value of Window.Current.Content will
        /// be set to the Content value of the Frame. This is not a navigation operation,
        /// it will only result in a visible effect for the user. 
        /// </summary>
        /// <remarks>
        /// This feature was created for the sake of the splash screen which will
        /// be seamlessly visible when Include is selected and this Frame is used 
        /// as the initial visual. 
        /// </remarks>
        Include,
        Exclude
    }
}
