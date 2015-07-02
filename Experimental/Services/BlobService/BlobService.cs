using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Dispatchr.Client.Services
{
    public class BlobService 
    {
        string _containerName;
        BlobHelper _helper = default(BlobHelper);
        public BlobService(string blobAccountName, string blobAccessKey, string transferGroupName, string blobContainerName)
        {
            _containerName = blobContainerName;
            _helper = new BlobHelper(blobAccountName, blobAccessKey, transferGroupName);
        }

        public Uri GetReadPath(string name)
        {
            var expire = TimeSpan.FromDays(365 * 2);
            var read = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read;
            return _helper.Blob.GetSas(read, expire, _containerName, name);
        }

        public async Task UploadAsync(StorageFile source, string name = null)
        {
            name = name ?? source.Name;
            await _helper.Blob.UploadAsync(source, _containerName, name);
        }
    }
}
