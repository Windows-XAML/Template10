namespace Template10.Common
{
    public interface IPropertyBagEx2
    {
        IPropertyBagExPersistenceStrategy PersistenceStrategy { get; set; }
        IPropertyBagExSerializationStrategy SerializationStrategy { get; set; }
    }

    public interface IPropertyBagExSimple 
    {
        void Write(string key, string value);
        string Read(string key);
    }
}