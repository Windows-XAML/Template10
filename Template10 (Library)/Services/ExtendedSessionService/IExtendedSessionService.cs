namespace Template10.Services.ExtendedSessionService
{
    public interface IExtendedSessionService
    {
        ExtendedSessionService.ClosingStatuses ApplicationClosingStatus { get; }
    }
}