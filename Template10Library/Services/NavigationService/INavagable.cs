using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public interface INavigable
    {
        void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state);
        Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending);
        void OnNavigatingFrom(Services.NavigationService.NavigatingEventArgs args);
        NavigationService NavigationService { get; set; }

        /// <summary>
        /// Used by NavigationService when NavigationCacheMode is Enabled, will load state when possible.
        /// </summary>
        string Identifier { get; set; }
    }
}
