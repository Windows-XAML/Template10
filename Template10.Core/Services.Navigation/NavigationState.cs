using System;
using System.Threading.Tasks;
using Template10.Interfaces.Services.Navigation;

namespace Template10.Navigation
{
    public class NavigationState : Lazy<string>, INavigationState
    {
        INavigationService _navigationService;

        public NavigationState(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public Task<bool> LoadAsync(string name = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync(string name = null)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}