using System.Threading.Tasks;

namespace Template10.Services.FileService
{
    public class FileService : IFileService
    {
        protected FileHelper Helper { get; private set; }
        protected StorageStrategies DefaultLocation { get; }

        public FileService(
            StorageStrategies location = StorageStrategies.Local,
            SerializationService.ISerializationService serializer = null)
        {
            DefaultLocation = location;
            Helper = new FileHelper(serializer ?? SerializationService.SerializationHelper.Json);
        }

        public async Task<T> ReadFileAsync<T>(string path) => await Helper.ReadFileAsync<T>(path, DefaultLocation);
        public async Task WriteFileAsync<T>(T item, string path) => await Helper.WriteFileAsync<T>(path, item, DefaultLocation);
        public async Task<bool> FileExistsAsync(string path) => await Helper.FileExistsAsync(path, DefaultLocation);
        public async Task<bool> DeleteFileAsync(string path) => await Helper.DeleteFileAsync(path, DefaultLocation);
    }
}
