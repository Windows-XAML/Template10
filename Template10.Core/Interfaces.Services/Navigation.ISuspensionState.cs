using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template10.Interfaces.Services.Navigation
{
    public interface ISuspensionState : IDictionary<string, object>
    {
        ISuspensionState Mark();
        Task LoadAsync(string name = null);
        Task SaveAsync(string name = null);
        T TryGet<T>(string key);
        void TrySet<T>(string key, T value);
    }
}