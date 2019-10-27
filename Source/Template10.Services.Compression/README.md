# CompressionService
`CompressionService` is a handy compression service for WinRT. Part of Template 10, a Library of Helpers for UWP. The current implementation supports the gZip algorithm.

```csharp
public interface ICompressionService
{
    string Unzip(string value, CompressionMethods method = CompressionMethods.gzip);
    string Zip(string value, CompressionMethods method = CompressionMethods.gzip);
}
```

