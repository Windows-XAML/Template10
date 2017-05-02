namespace Template10.Services.SecretService
{
	public abstract class SecretServiceBase
    {
        public SecretServiceBase() => Helper = new SecretHelper();

        public SecretHelper Helper { get; private set; }
    }
}
