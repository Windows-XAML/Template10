using Prism.Windows.Services.Serialization;

namespace Prism.Windows.Services.Settings
{
    public interface ISettingsAdapter
    {
        string ReadString(string key);
        void WriteString(string key, string value);
        ISerializationService SerializationService { get; }
    }
}
