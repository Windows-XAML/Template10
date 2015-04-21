using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template10.Repositories
{
    public abstract class RepositoryBase<T>
        where T : Models.IKeyedModel
    {
        readonly string CACHEKEY;
        Services.FileService.FileService _fileService;

        public RepositoryBase(string cachekey)
        {
            if (string.IsNullOrEmpty(cachekey))
                throw new ArgumentNullException(nameof(cachekey));
            CACHEKEY = cachekey;
            _fileService = new Services.FileService.FileService();
        }

        public async Task<List<T>> GetAsync()
        {
            return await _fileService.ReadAsync<T>(CACHEKEY);
        }

        public async Task<T> GetAsync(string key)
        {
            var items = await this.GetAsync();
            return items.FirstOrDefault(x => x.Key.Equals(key));
        }

        public async Task SaveAsync(List<T> items)
        {
            if (items == null)
                return;
            await _fileService.WriteAsync<T>(CACHEKEY, items);
        }
    }
}
