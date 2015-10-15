using Sample.DataModels;
using Sample.Extensions;
using Sample.Services.GithubService;
using Sample.Shared;
using Sample.Shared;
using System;
using Template10.Mvvm;
using Windows.System;

namespace Sample.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RepositorySource _repositoriesSource;

        private DelegateCommand<string> _openCommand;

        private string _query = "Template 10";

        private DelegateCommand _queryCommand;

        public MainViewModel(IGithubService service)
        {
            _repositoriesSource = new RepositorySource(service);
            Repositories = new IncrementalLoadingCollection<Repository, RepositorySource>(_repositoriesSource);
        }

        public DelegateCommand<string> OpenCommand
        {
            get
            {
                this._openCommand = this._openCommand ?? new DelegateCommand<string>(async url =>
                {
                    await Launcher.LaunchUriAsync(new Uri(url));
                });
                return this._openCommand;
            }
        }

        public string Query
        {
            get
            {
                return this._query;
            }
            set
            {
                this.Set(ref this._query, value);
                this.QueryCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand QueryCommand
        {
            get
            {
                this._queryCommand = this._queryCommand ?? new DelegateCommand(() =>
                {
                    this._repositoriesSource.Query = this.Query;
                    Repositories.Refresh();
                }, () => this.Query.IsBlank() == false);
                return this._queryCommand;
            }
        }

        public IncrementalLoadingCollection<Repository, RepositorySource> Repositories
        {
            get;
            private set;
        }
    }
}