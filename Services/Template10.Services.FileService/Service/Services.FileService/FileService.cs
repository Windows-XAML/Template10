using System.Threading.Tasks;

namespace Template10.Services.FileService
{
    public class FileService : FileServiceBase, IFileService
    {
        public async Task<T> ReadTempFile<T>(string path)
        {
            return await Helper.ReadFileAsync<T>(path, StorageStrategies.Temporary);
        }

        public async Task WriteTempFile<T>(T item, string path)
        {
            await Helper.WriteFileAsync<T>(path, item, StorageStrategies.Temporary);
        }

        public async Task<T> ReadLocalFile<T>(string path)
        {
            return await Helper.ReadFileAsync<T>(path, StorageStrategies.Local);
        }

        public async Task WriteLocalFile<T>(T item, string path)
        {
            await Helper.WriteFileAsync<T>(path, item, StorageStrategies.Local);
        }

        public async Task<T> ReadRoamingFile<T>(string path)
        {
            return await Helper.ReadFileAsync<T>(path, StorageStrategies.Roaming);
        }

        public async Task WriteRoamingFile<T>(T item, string path)
        {
            await Helper.WriteFileAsync<T>(path, item, StorageStrategies.Roaming);
        }
    }
}
