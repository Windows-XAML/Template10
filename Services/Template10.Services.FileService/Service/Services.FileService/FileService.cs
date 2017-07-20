using System.Threading.Tasks;

namespace Template10.Services.FileService
{
    public class FileService : IFileService
    {
        protected FileHelper Helper { get; private set; }
        protected StorageStrategies DefaultLocation { get; private set; } = StorageStrategies.Local;
        public FileService(StorageStrategies location) : this() => DefaultLocation = location;
        public FileService() => Helper = new FileHelper();

        public async Task<T> ReadFileAsync<T>(string path) => await Helper.ReadFileAsync<T>(path, DefaultLocation);
        public async Task WriteFileAsync<T>(T item, string path) => await Helper.WriteFileAsync<T>(path, item, DefaultLocation);
        public async Task<bool> FileExistsAsync(string path) => await Helper.FileExistsAsync(path, DefaultLocation);
        public async Task<bool> DeleteFileAsync(string path) => await Helper.DeleteFileAsync(path, DefaultLocation);
    }
}
