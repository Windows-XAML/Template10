using System.Threading.Tasks;

namespace Template10.Services.FileService
{
    public interface IFileService
    {
        StorageStrategies DefaultLocation { get; set; }
        Task<T> ReadFileAsync<T>(string path);
        Task WriteFileAsync<T>(T item, string path);
        Task<bool> FileExistsAsync(string path);
        Task<bool> DeleteFileAsync(string path);
    }
}