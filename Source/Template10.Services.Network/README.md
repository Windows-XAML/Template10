# NetworkService

The `NetworkService` provides simplified API to determine network and internet availability.

```csharp
public interface INetworkService 
{
    Task<bool> GetIsInternetAvailableAsync();
    Task<bool> GetIsNetworkAvailableAsync();
}
```