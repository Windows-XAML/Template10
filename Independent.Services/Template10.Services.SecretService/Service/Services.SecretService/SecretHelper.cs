﻿using System.Linq;
using Windows.Security.Credentials;

namespace Template10.Services.SecretService
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
            }
            else
            {
                var credential = new PasswordCredential(container, key, secret);
                _vault.Add(credential);
            }
        }
    }
}
