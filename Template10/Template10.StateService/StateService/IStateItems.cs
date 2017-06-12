using Windows.Foundation.Collections;

namespace Template10.Services.StateService
{
    public interface IStateItems : IPropertySet
    {
        bool TryGet<T>(string key, out T value);
        T Get<T>(string key);
    }
}