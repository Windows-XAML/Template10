# WebService

The `WebService` provides simplified access to REST based services and provides the means to automatically serialize and deserialize payloads to/from JSON. Leverages the [Template 10 Serialization Service](https://www.nuget.org/packages/Template10.Services.Serialization/).

```csharp
public interface IWebApiAdapter
{
    Task DeleteAsync(Uri path);
    Task<string> GetAsync(Uri path);
    Task PostAsync(Uri path, string payload);
    Task PutAsync(Uri path, string payload);
}

public interface IWebApiService
{
    Task<string> GetAsync(Uri path);
    Task PutAsync<T>(Uri path, T payload);
    Task PostAsync<T>(Uri path, T payload);
    Task DeleteAsync(Uri path);
}
```