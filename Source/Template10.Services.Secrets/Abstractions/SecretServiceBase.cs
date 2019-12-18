namespace Template10.Services
{
    public abstract class SecretServiceBase
    {
        static SecretServiceBase()
        {
            _helper = new SecretHelper();
        }

        private static readonly SecretHelper _helper;
        public SecretHelper Helper => _helper;
    }
}
