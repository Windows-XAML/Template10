namespace Template10.Services.Logging
{
    public interface ILoggingService
    {
        bool Enabled { get; set; }
        DebugWriteDelegate WriteLine { get; set; }
    }
}