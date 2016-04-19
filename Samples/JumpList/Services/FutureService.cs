using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace Template10.Samples.JumpListSample.Services
{
    class FutureService
    {
        StorageItemAccessList _future;

        public FutureService()
        {
            _future = StorageApplicationPermissions.FutureAccessList;
        }

        public async Task<StorageFile> GetAsync(string path)
        {
            if (!_future.Entries.Any(x => x.Metadata.Equals(path)))
                return null;
            var token = _future.Entries.First(x => x.Metadata.Equals(path)).Token;
            return await _future.GetFileAsync(token);
        }

        public void Add(StorageFile file)
        {
            if (!_future.Entries.Any(x => x.Metadata.Equals(file.Path)))
                _future.Add(file, file.Path);
        }
    }
}
