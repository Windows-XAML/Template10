using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template10.Suspension
{
    public interface ISuspensionState : IDictionary<string, object>
    {
        Task LoadAsync(string name = null);
        Task SaveAsync(string name = null);
        T TryGet<T>(string name);
    }
}