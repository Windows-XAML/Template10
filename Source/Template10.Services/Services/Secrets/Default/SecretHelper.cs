using System.Linq;
using Windows.Security.Credentials;

namespace Template10.Services.Secrets
{
    public class SecretHelper
    {
        // https://msdn.microsoft.com/en-us/library/windows/apps/windows.security.credentials.passwordvault.aspx

        static PasswordVault _vault;

        static SecretHelper()
        {
            _vault = new PasswordVault();
        }

        public string ReadSecret(string key)
        {
            return ReadSecret(GetType().ToString(), key);
        }

        public string ReadSecret(string container, string key)
        {
            if (_vault.RetrieveAll().Any(x => x.Resource == container && x.UserName == key))
            {
                var credential = _vault.Retrieve(container, key);
                credential.RetrievePassword();
                return credential.Password;
            }
            else
            {
                return string.Empty;
            }
        }

        public void WriteSecret(string key, string secret)
        {
            WriteSecret(GetType().ToString(), key, secret);
        }

        public void WriteSecret(string container, string key, string secret)
        {
            if (_vault.RetrieveAll().Any(x => x.Resource == container && x.UserName == key))
            {
                var credential = _vault.Retrieve(container, key);
                credential.RetrievePassword();
                credential.Password = secret;
                _vault.Add(credential);
            }
            else
            {
                var credential = new PasswordCredential(container, key, secret);
                _vault.Add(credential);
            }
        }

        public bool IsSecretExistsForKey(string key, bool RemoveIfExists = false)
        {
            var container = GetType().ToString();

            if (_vault.RetrieveAll().Any(x => x.Resource == container && x.UserName == key))
            {
                var credential = _vault.Retrieve(container, key);
                credential.RetrievePassword();

                if (credential.Password.Length > 0)
                {
                    if (RemoveIfExists) _vault.Remove(credential);
                    return true;
                }
                else
                {
                    // a blank key shouldn't exist, but who knows ...
                    _vault.Remove(credential);
                }
            }

            return false;
        }
    }
}
