using System.Threading.Tasks;

namespace Template10.Services.FileService
{
    public class FileService : IFileService
    {
        public static IFileService Create()
        {
            return new FileService(new Serialization.JsonSerializationService());
        }

        protected FileHelper Helper { get; private set; }
        public FileService(Serialization.ISerializationService serializer)
        {
            Helper = new FileHelper(serializer ?? new Serialization.JsonSerializationService());
        }

        public StorageStrategies DefaultLocation { get; set; } = StorageStrategies.Local;
        public async Task<T> ReadFileAsync<T>(string path) => await Helper.ReadFileAsync<T>(path, DefaultLocation);
        public async Task WriteFileAsync<T>(T item, string path) => await Helper.WriteFileAsync<T>(path, item, DefaultLocation);
        public async Task<bool> FileExistsAsync(string path) => await Helper.FileExistsAsync(path, DefaultLocation);
        public async Task<bool> DeleteFileAsync(string path) => await Helper.DeleteFileAsync(path, DefaultLocation);
    }
}
