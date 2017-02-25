namespace Template10.Services.SecretService
{
    public interface ISecretService
    {
        string ReadSecret(string key);
        string ReadSecret(string container, string key);
        void WriteSecret(string key, string secret);
        void WriteSecret(string container, string key, string secret);
    }
}