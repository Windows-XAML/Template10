using Template10.Common;

namespace Template10.Common
{
    public class EmptyPropertyBagSerializationStrategy : IPropertyBagExSerializationStrategy
    {
        internal EmptyPropertyBagSerializationStrategy()
        {
            // empty
        }
        public object Serialize(object obj) => obj;
        public T Deserialize<T>(object value) => (T)value;
    }
}
