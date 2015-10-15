using Sample.Design.Services;
using Sample.Services;
using System;
using Windows.ApplicationModel;

namespace Sample.ViewModels
{
    public class ViewModelLocator
    {
        private Lazy<MainViewModel> _main;
        
        public ViewModelLocator()
        {
            if (DesignMode.DesignModeEnabled)
            {
                // in design mode, use fake data.
                _main = new Lazy<MainViewModel>(() => new MainViewModel(new FakeGithubService()));
            }
            else
            {
                // in real running, use the real service to load real data.
                _main = new Lazy<MainViewModel>(() => new MainViewModel(new GithubService()));
            }
        }

        public MainViewModel Main
        {
            get
            {
                return _main.Value;
            }
        }
    }
}