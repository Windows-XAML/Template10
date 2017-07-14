using System.Threading.Tasks;
using Template10.Portable;
using Template10.Portable.Common;

namespace Template10.Common.PersistedDictionary
{
    public interface IPersistedDictionaryFactory
    {
        Task<IPersistedDictionary> CreateAsync(string key);
        Task<IPersistedDictionary> CreateAsync(string key, string container);
    }

    public interface IPersistedDictionary : IPropertyBagAsync
    {
        string Name { get; }
    }
}
