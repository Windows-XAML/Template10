# SecretService

The `SecretService` securely stores secret strings. Secret strings are any string value you don't want to store as plain text (like in a file or database or setting). The `SecretService` is a simple wrapper for Windows' [Credential Manager](https://msdn.microsoft.com/en-us/library/windows/desktop/aa374792(v=vs.85).aspx). The Credential Manager handles the encryption part and secures the result internally. This is the Windows standard for securing strings.

````csharp

public interface ISecretService
{
    string ReadSecret(string key);
    string ReadSecret(string container, string key);
    void WriteSecret(string key, string secret);
    void WriteSecret(string container, string key, string secret);
}

````


### Usage Example:

````csharp

SecretService service = new SecretService();

// save a secret in the default container
public string Token
{
    get { return service.ReadSecret(nameof(Token)); }
    set { service.WriteSecret(nameof(Token), value); }
}

// save a secret in a custom container
public string Token
{
    get { return service.ReadSecret("OAuth", nameof(Token)); }
    set { service.WriteSecret("OAuth", nameof(Token), value); }
}

````
The purpose of a custom container is if you want to reuse a key without overwriting another value. For example if you store two tokens, they can both be stored as "Token" in separate containers. 

Saving OAuth tokens is a good use of the `SecretService`. If you maintain any credentials, keys, hash values, or tokens for the user or your services, it's recommended that you consider storing the values using this service.