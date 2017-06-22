using System.Threading.Tasks;

namespace Template10.Strategies.ExtendedSessionStrategy
{
    public interface IExtendedSessionStrategyInternal : IExtendedSessionStrategy
    {
        SessionKinds CurrentKind { get; }
        bool IsActive { get; }
        bool IsStarted { get; }
        bool IsRevoked { get; }
        int Progress { get; }

        Task<bool> StartSaveDataAsync();
        Task<bool> StartUnspecifiedAsync();
    }
}
