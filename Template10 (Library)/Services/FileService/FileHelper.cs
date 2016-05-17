using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace Template10.Services.FileService
{
    public static class FileHelper
    {
        /// <summary>Returns if a file is found in the specified storage strategy</summary>
        /// <param name="key">Path of the file in storage</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>Boolean: true if found, false if not found</returns>
        public static async Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local) => (await GetIfFileExistsAsync(key, location)) != null;

        public static async Task<bool> FileExistsAsync(string key, StorageFolder folder) => (await GetIfFileExistsAsync(key, folder)) != null;

        /// <summary>Deletes a file in the specified storage strategy</summary>
        /// <param name="key">Path of the file in storage</param>
        /// <param name="location">Location storage strategy</param>
        public static async Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local)
        {
            var file = await GetIfFileExistsAsync(key, location);
            if (file != null)
                await file.DeleteAsync();
            return !(await FileExistsAsync(key, location));
        }

        /// <summary>Reads and deserializes a file into specified type T</summary>
        /// <typeparam name="T">Specified type into which to deserialize file content</typeparam>
        /// <param name="key">Path to the file in storage</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>Specified type T</returns>
        public static async Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local)
        {
            try
            {
                // fetch file
                var file = await GetIfFileExistsAsync(key, location);
                if (file == null)
                    return default(T);
                // read content
                var readValue = await FileIO.ReadTextAsync(file);
                // convert to obj
                var _Result = Deserialize<T>(readValue);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Serializes an object and write to file in specified storage strategy</summary>
        /// <typeparam name="T">Specified type of object to serialize</typeparam>
        /// <param name="key">Path to the file in storage</param>
        /// <param name="value">Instance of object to be serialized and written</param>
        /// <param name="location">Location storage strategy</param>
        public static async Task<bool> WriteFileAsync<T>(string key, T value, StorageStrategies location = StorageStrategies.Local,
            CreationCollisionOption option = CreationCollisionOption.ReplaceExisting)
        {
            // create file
            var file = await CreateFileAsync(key, location, option);
            // convert to string
            var serializedValue = Serialize(value);
            // save string to file
            await FileIO.WriteTextAsync(file, serializedValue);
            // result
            return await FileExistsAsync(key, location);
        }

        private static async Task<StorageFile> CreateFileAsync(string key, StorageStrategies location = StorageStrategies.Local,
            CreationCollisionOption option = CreationCollisionOption.OpenIfExists)
        {
            switch (location)
            {
                case StorageStrategies.Local:
                    return await ApplicationData.Current.LocalFolder.CreateFileAsync(key, option);
                case StorageStrategies.Roaming:
                    return await ApplicationData.Current.RoamingFolder.CreateFileAsync(key, option);
                case StorageStrategies.Temporary:
                    return await ApplicationData.Current.TemporaryFolder.CreateFileAsync(key, option);
                default:
                    throw new NotSupportedException(location.ToString());
            }
        }

        private static async Task<StorageFile> GetIfFileExistsAsync(string key, StorageFolder folder)
        {
            StorageFile retval;
            try
            {
                retval = await folder.GetFileAsync(key);
            }
            catch (System.IO.FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine("GetIfFileExistsAsync:FileNotFoundException");
                return null;
            }
            return retval;
        }

        /// <summary>Returns a file if it is found in the specified storage strategy</summary>
        /// <param name="key">Path of the file in storage</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>StorageFile</returns>
        private static async Task<StorageFile> GetIfFileExistsAsync(string key,
            StorageStrategies location = StorageStrategies.Local)
        {
            StorageFile retval;
            try
            {
                switch (location)
                {
                    case StorageStrategies.Local:
                        retval = await ApplicationData.Current.LocalFolder.TryGetItemAsync(key) as StorageFile;
                        break;
                    case StorageStrategies.Roaming:
                        retval = await ApplicationData.Current.RoamingFolder.TryGetItemAsync(key) as StorageFile;
                        break;
                    case StorageStrategies.Temporary:
                        retval = await ApplicationData.Current.TemporaryFolder.TryGetItemAsync(key) as StorageFile;
                        break;
                    default:
                        throw new NotSupportedException(location.ToString());
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine("GetIfFileExistsAsync:FileNotFoundException");
                return null;
            }
            return retval;
        }

        private static string Serialize<T>(T item) => JsonConvert.SerializeObject(item, Formatting.None, new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
        });

        private static T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}
