namespace Template10.Services.Logging
{
    public interface ILoggingService
    {
        LoggingService.DebugWriteDelegate WriteLine { get; set; }
    }
}