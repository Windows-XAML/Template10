# SecretService

The `SecretService` makes use of the `PasswordVault` class (see [here](https://docs.microsoft.com/en-us/uwp/api/Windows.Security.Credentials.PasswordVault) for more details) to store a connection string in a credential locker.

```csharp
public interface ISecretService
{
    string ConnectionString { get; set; }
}
```