namespace Template10.Services.NavigationService
{
    interface IStatefulNavigationParameter
    {
        /// <summary>
        /// Gets string representation of the state of this object
        /// </summary>
        /// <returns>A string representation that can be stored for later hydration of an instance of this object with similar functionality</returns>
        string GetState();

        /// <summary>
        /// Populates the internal state of this object from the state returned by <see cref="GetState"/>
        /// </summary>
        /// <param name="state">The state to restore from.</param>
        /// <remarks>In a View with a ViewModel that implements <see cref="IStatefulNavigationParameter"/>, 
        /// call this method from <see cref="Windows.UI.Xaml.Controls.Page.OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs)"/>
        /// to set your ViewModel to an instance representative of the previously-saved state</remarks>
        void PopulateFromState(string state);
    }
}
