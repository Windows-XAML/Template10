namespace Template10.Services.Logging
{
    public class DefaultAdapter : ILoggingAdapter
    {
        public void Log(string text, Severities severity)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
    }
}
