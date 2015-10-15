using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Services.FileService
{
    public class FileService : IFileService
    {
        FileHelper _helper = new FileHelper();

        public Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local) => _helper.FileExistsAsync(key, location);

        public Task<bool> FileExistsAsync(string key, StorageFolder folder) => _helper.FileExistsAsync(key, folder);

        public Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local) => _helper.DeleteFileAsync(key, location);

        public Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local) => _helper.ReadFileAsync<T>(key, location);

        public Task<bool> WriteFileAsync<T>(string key, T value, StorageStrategies location = StorageStrategies.Local) => _helper.WriteFileAsync<T>(key, value, location);
    }
}
