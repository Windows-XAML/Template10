using Template10.Samples.IncrementalLoadingSample.Services.GithubService;
using System;
using Windows.ApplicationModel;

namespace Template10.Samples.IncrementalLoadingSample.ViewModels
{
    public class ViewModelLocator
    {
        private Lazy<MainViewModel> _main;
        
        public ViewModelLocator()
        {
            if (DesignMode.DesignModeEnabled)
            {
                // in design mode, use fake data.
                _main = new Lazy<MainViewModel>(() => new MainViewModel(new GithubService4DesignTime()));
            }
            else
            {
                // in real running, use the real service to load real data.
                _main = new Lazy<MainViewModel>(() => new MainViewModel(new GithubService4RunTime()));
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