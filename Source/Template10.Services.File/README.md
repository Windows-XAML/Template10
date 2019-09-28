# FileService
`FileService` simplifies the common ways of interacting with files by abstracting the storage types to `Local`, `Roaming` and `Temporary`. In doing so, the service takes care of correctly creating the appropriate paths. All storage strategies are within the app's own data directory. All calls that allow a storage strategy to be specified default to local storage.

```csharp
// Deletes a file in the specified storage strategy. key is the path of the file in storage. This
// is safe to call even if the file doesn't exist already. Returns true if the deletion operation
// was successful (i.e. the file doesn't exist).
async Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local)

// This checks for the existence of a file, either in the specified storage strategy or in the
// specified folder.
async Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local)
async Task<bool> FileExistsAsync(string key, StorageFolder folder)

// ReadFileAsync reads and deserializes a file into specified type T. T specifies the type into which
// to deserialize the file content while key is the path to the file within the specified storage strategy.
async Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local)

// WriteFileAsync serializes an object (of specified type T) and writes it to file in the specified storage
// strategy. Key specifies the path to the file within the storage strategy. You can also alter the behaviour
// if the code if the destination file already exists. The method returns a boolean to indicate whether or not
// the file exists.
async Task<bool> WriteFileAsync<T>(string key, T value, StorageStrategies location = StorageStrategies.Local,
            CreationCollisionOption option = CreationCollisionOption.ReplaceExisting)
```