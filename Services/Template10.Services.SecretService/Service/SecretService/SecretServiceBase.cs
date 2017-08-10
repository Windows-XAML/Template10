namespace Template10.Services.Secrets
{
    public abstract class SecretServiceBase
    {
        public SecretServiceBase() => Helper = new SecretHelper();

        public SecretHelper Helper { get; private set; }
    }
}
