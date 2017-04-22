namespace Template10.Services.KeyboardService
{
    public abstract class KeyboardServiceBase
    {
        public KeyboardServiceBase() => Helper = new KeyboardHelper();

        protected KeyboardHelper Helper { get; private set; }
    }

}
