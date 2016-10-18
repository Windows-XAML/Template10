using System;
using System.Threading.Tasks;

namespace Template10.Navigation
{
    public class NavigationState : Lazy<string>, INavigationState
    {
        INavigationService _navigationService;

        public NavigationState(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public Task LoadAsync(string name = null)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(string name = null)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}