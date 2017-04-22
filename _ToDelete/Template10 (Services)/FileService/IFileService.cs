using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.FileService
{
    public interface IFileService
    {
        Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local);
        Task<bool> FileExistsAsync(string key, Windows.Storage.StorageFolder folder);
        Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local);
        Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local);
        Task<bool> WriteFileAsync<T>(string key, T value, StorageStrategies location = StorageStrategies.Local);
    }

    public enum StorageStrategies { Local, Roaming, Temporary }
}
