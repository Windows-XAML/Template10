using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.ViewModels
{
    class MainPageViewModel : INavigatedAware
    {
        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            // empty
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            // empty
        }
    }
}
