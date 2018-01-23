using System.Threading.Tasks;

namespace Template10.Application
{
    public interface IBootStrapper
    {
        Task InitializeAsync(StartArgs args);
        Task StartAsync(StartArgs args, StartKinds activate);
    }
}
