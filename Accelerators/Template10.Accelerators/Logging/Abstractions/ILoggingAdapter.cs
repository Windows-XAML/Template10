namespace Template10.Services.Logging
{
    public interface ILoggingAdapter
    {
        void Log(string text, Severities severity);
    }
}
