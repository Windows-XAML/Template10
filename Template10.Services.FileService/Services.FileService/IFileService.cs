using System.Threading.Tasks;

namespace Template10.Services.FileService
{
    public interface IFileService
    {
        Task<T> ReadLocalFile<T>(string path);
        Task<T> ReadRoamingFile<T>(string path);
        Task<T> ReadTempFile<T>(string path);
        Task WriteLocalFile<T>(T item, string path);
        Task WriteRoamingFile<T>(T item, string path);
        Task WriteTempFile<T>(T item, string path);
    }
}