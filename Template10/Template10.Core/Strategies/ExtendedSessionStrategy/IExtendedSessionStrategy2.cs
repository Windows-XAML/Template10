using System.Threading.Tasks;

namespace Template10.Strategies
{
    public interface IExtendedSessionStrategy2 
    {
        ExtendedSessionKinds CurrentKind { get; }
        bool IsActive { get; }
        bool IsStarted { get; }
        bool IsRevoked { get; }
        int Progress { get; }

        Task<bool> StartSaveDataAsync();
        Task<bool> StartUnspecifiedAsync();
    }
}
