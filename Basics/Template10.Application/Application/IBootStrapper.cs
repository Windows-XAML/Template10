using System.Threading.Tasks;

namespace Template10.Application
{
    public interface IBootStrapper
    {
        void Initialize(StartArgs args);
        Task InitializeAsync(StartArgs args);
        Task StartAsync(StartArgs args, StartKinds activate);
    }
}
