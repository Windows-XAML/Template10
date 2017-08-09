using Template10.Common;

namespace Template10.Core
{
    public class EmptyPropertyBagSerializationStrategy : IPropertyBagExSerializationStrategy
    {
        public object Serialize(object obj) => obj;
        public T Deserialize<T>(object value) => (T)value;
    }
}
