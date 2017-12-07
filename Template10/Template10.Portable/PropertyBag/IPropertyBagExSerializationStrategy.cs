namespace Template10.Common
{
    public interface IPropertyBagExSerializationStrategy
    {
        object Serialize(object obj);
        T Deserialize<T>(object value);
    }
}