using Template10.Samples.IncrementalLoadingSample.Models;
using Template10.Samples.IncrementalLoadingSample.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template10.Samples.IncrementalLoadingSample.Services.GithubService
{
    public class GithubIncrementalItemSource : IncrementalItemSourceBase<Repository>
    {
        private readonly IGithubService _service;

        private int _currentPage = 0;

        public GithubIncrementalItemSource(IGithubService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            this._service = service;
        }

        public string Query
        {
            get;
            set;
        }

        protected internal override async Task LoadMoreItemsAsync(ICollection<Repository> collection, uint suggestLoadCount)
        {
            try
            {
                int nextPage = this._currentPage + 1;
                GithubQueryResult<Repository> result = await this._service.GetRepositoriesAsync(this.Query, nextPage);
                if (result.ErrorMessage == null)// no error
                {
                    Repository[] repositories = result.Items;
                    foreach (Repository repository in repositories)
                    {
                        if (collection.Any(temp => temp.Id == repository.Id))
                        {
                            // repository had loaded.
                        }
                        else
                        {
                            collection.Add(repository);
                        }
                    }

                    if (collection.Count >= result.TotalCount)
                    {
                        // had load all the datas, raise has more items.
                        this.RaiseHasMoreItemsChanged(false);
                    }
                }

                // load and add success, set current page to nextpage.
                this._currentPage = nextPage;
            }
            catch
            {
                // you can log the exception here.
            }
        }
         
        protected internal override void OnRefresh(ICollection<Repository> collection)
        {
            // don't remove this code, it is necessary.
            base.OnRefresh(collection);

            // reset current page.
            this._currentPage = 0;
        }
    }
}
