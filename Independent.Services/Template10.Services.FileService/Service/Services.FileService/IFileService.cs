using System.Threading.Tasks;

namespace Template10.Services.FileService
{
	public interface IFileService
    {
        Task<T> ReadFileAsync<T>(string path, StorageStrategies? location = null);
        Task<T> ReadLocalFileAsync<T>(string path);
        Task<T> ReadRoamingFileAsync<T>(string path);
        Task<T> ReadTempFileAsync<T>(string path);

        Task WriteFileAsync<T>(T item, string path, StorageStrategies? location = null);
        Task WriteLocalFileAsync<T>(T item, string path);
        Task WriteRoamingFileAsync<T>(T item, string path);
        Task WriteTempFileAsync<T>(T item, string path);

        Task<bool> FileExistsAsync(string path, StorageStrategies? location = null);
        Task<bool> LocalFileExistsAsync(string path);
        Task<bool> RoamingFileExistsAsync(string path);
        Task<bool> TempFileExistsAsync(string path);

        Task<bool> DeleteFileAsync(string path, StorageStrategies? location = null);
        Task<bool> DeleteLocalFileAsync(string path);
        Task<bool> DeleteRoamingFileAsync(string path);
        Task<bool> DeleteTempFileAsync(string path);
    }
}