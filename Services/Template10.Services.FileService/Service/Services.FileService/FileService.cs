using System.Threading.Tasks;

namespace Template10.Services.FileService
{
    public class FileService : IFileService
    {
        protected FileHelper Helper { get; private set; }
        protected StorageStrategies DefaultLocation { get; private set; } = StorageStrategies.Local;

        public FileService() => Helper = new FileHelper();
        public FileService(StorageStrategies location) : this() => DefaultLocation = location;

        public async Task<T> ReadFileAsync<T>(string path, StorageStrategies? location = null) => await Helper.ReadFileAsync<T>(path, location ?? DefaultLocation);
        public async Task<T> ReadTempFileAsync<T>(string path) => await ReadFileAsync<T>(path, StorageStrategies.Temporary);
        public async Task<T> ReadLocalFileAsync<T>(string path) => await ReadFileAsync<T>(path, StorageStrategies.Local);
        public async Task<T> ReadRoamingFileAsync<T>(string path) => await ReadFileAsync<T>(path, StorageStrategies.Roaming);

        public async Task WriteFileAsync<T>(T item, string path, StorageStrategies? location = null) => await Helper.WriteFileAsync<T>(path, item, location ?? DefaultLocation);
        public async Task WriteTempFileAsync<T>(T item, string path) => await WriteFileAsync(item, path, StorageStrategies.Temporary);
        public async Task WriteLocalFileAsync<T>(T item, string path) => await WriteFileAsync(item, path, StorageStrategies.Local);
        public async Task WriteRoamingFileAsync<T>(T item, string path) => await WriteFileAsync(item, path, StorageStrategies.Roaming);

        public async Task<bool> FileExistsAsync(string path, StorageStrategies? location = null) => await Helper.FileExistsAsync(path, location ?? DefaultLocation);
        public async Task<bool> LocalFileExistsAsync(string path) => await FileExistsAsync(path, StorageStrategies.Local);
        public async Task<bool> RoamingFileExistsAsync(string path) => await FileExistsAsync(path, StorageStrategies.Roaming);
        public async Task<bool> TempFileExistsAsync(string path) => await FileExistsAsync(path, StorageStrategies.Temporary);

        public async Task<bool> DeleteFileAsync(string path, StorageStrategies? location = null) => await Helper.DeleteFileAsync(path, location ?? DefaultLocation);
        public async Task<bool> DeleteLocalFileAsync(string path) => await DeleteFileAsync(path, StorageStrategies.Local);
        public async Task<bool> DeleteRoamingFileAsync(string path) => await DeleteFileAsync(path, StorageStrategies.Roaming);
        public async Task<bool> DeleteTempFileAsync(string path) => await DeleteFileAsync(path, StorageStrategies.Temporary);
    }
}
