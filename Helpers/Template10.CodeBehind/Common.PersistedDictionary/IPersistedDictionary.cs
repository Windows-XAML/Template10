using Template10.Portable;
using Template10.Portable.Common;

namespace Template10.Common.PersistedDictionary
{
    public interface IPersistedDictionary : IPropertyBagAsync
    {
        string Name { get; }
    }
}
