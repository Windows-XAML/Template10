namespace Template10.Services.Secrets
{
    public abstract class SecretServiceBase
    {
        static SecretServiceBase()
        {
            _helper = new SecretHelper();
        }

        static SecretHelper _helper;
        public SecretHelper Helper => _helper;
    }
}
