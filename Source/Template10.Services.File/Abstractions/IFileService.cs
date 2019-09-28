using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Services
{
    public interface IFileService
    {
        Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local);
        Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local);
        Task<bool> FileExistsAsync(string key, StorageFolder folder);
        Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local);
        Task<string> ReadStringAsync(string key, StorageStrategies location = StorageStrategies.Local);
        Task<bool> WriteFileAsync<T>(string key, T value, StorageStrategies location = StorageStrategies.Local, CreationCollisionOption option = CreationCollisionOption.ReplaceExisting);
        Task<bool> WriteStringAsync(string key, string value, StorageStrategies location = StorageStrategies.Local, CreationCollisionOption option = CreationCollisionOption.ReplaceExisting);
    }
}